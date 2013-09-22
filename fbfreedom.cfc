component hint="http://www.github.com/jamie-pate/FuseBoxFreedom" {
    this.acl = {};
    this.access_level = "public";

    function init(circuit_or_scope=false, circuit_scope=false) {
        if (not isObject(arguments.circuit_or_scope) and isStruct(arguments.circuit_or_scope)) {
            arguments.scope = arguments.circuit_or_scope;
            arguments.circuit_or_scope = false;
            this.current_circuit = false;
            this.circuit_name = '<master circuit>';
            this.init_scope(arguments.scope);
        }
        if (isObject(arguments.circuit_or_scope)) {
            this.current_circuit = arguments.circuit_or_scope;
            this.current_circuit.circuit_scope = arguments.circuit_scope;
            arguments.scope = false;
            this.circuit_name = '<pending>';
            //set later during get_method_circuit
        } else {
            //main instance has no circuit
            this.current_circuit = false;
        }
        this.scope = arguments.scope;
        this.settings = createobject('component', 'cfcs.fbFreedomSettings');
        this.defaults(this.settings, {circuitpath=''});
        this.circuits = {};
        return this;
    }

    function init_scope(scope) {
        if (not structkeyexists(arguments.scope, 'attributes')) {
            arguments.scope.attributes = {};
        }
        this.defaults(arguments.scope.attributes, form);
        this.defaults(arguments.scope.attributes, url);
    }

    function structSet(structure, key, value, overwrite=false) {
        try {
            structinsert(structure, key, value, overwrite)
        } catch('expression') {}
    }

    function set_xfa(name, value, overwrite=true) {
        var cscope = this.current_circuit.circuit_scope;
        if (listLen(arguments.value, '.') eq 1) {
            arguments.value = this.circuit_name & '.' & arguments.value;
        }
        this.structSet(cscope, 'xfa', {}, false);
        this.structSet(cscope.xfa, name, value, overwrite);
    }

    function instantiate(class) {
        var classes = this.settings.classes;
        this.structSet(classes, arguments.class, arguments.class, false);
        return createobject('component', this.settings.classpath & classes[arguments.class]);
    }


    function copy_scope(src, dest) {
        if (isStruct(src) and isstruct(dest)) {
            structClear(dest);
            return this.defaults(dest, src);
        }
    }

    function get_method_circuit(fuseAction) {
        var path = listDeleteAt(arguments.fuseAction,
                                listlen(arguments.fuseAction, '.'),
                                '.');
        var circuit_name = path;
        var result = false;
        var scope = this.scope;
        var need_init = false;
        var scope_is_same = false;
        if (structKeyExists(this.settings.circuits, path)) {
            path = this.settings.circuits[path];
        }
        if (structKeyExists(this.circuits, path)) {
            result = this.circuits[path];
        } else {
            result = createObject('component', this.settings.circuitPath & path & '.circuit');
            result.filePath = replace(this.settings.circuitPath & path, '.', '/', 'all');
            result.path = this.settings.circuitPath & path;
            //propagate global variable scope
            result.scope = this.scope;
            result.fb.scope = this.scope;
            result.fb.circuit_name = circuit_name;
            this.settings.circuits[path] = result;
            need_init = true;
        }
        if (isObject(this.current_circuit)) {
            scope_is_same = result.fb.circuit_name eq this.circuit_name;
            scope = this.current_circuit.circuit_scope;
        }
        if (not scope_is_same) {
            this.copy_scope(scope, result.circuit_scope);
        }
        if (need_init) {
            result.init();
        }
        return result;
    }

    function _access_check(access_level, internalRequest) {
        if (access_level eq 'internal' and not internalRequest) {
            throw(message='Unable to access #circuit.fb.circuit_name#.#arguments.fuseAction#. Access is #access_level# only.',
                  type='fbf:access_denied');
        }
    }

    function access_check(fuseAction, internalRequest) {
        this._access_check(this.acl[arguments.fuseAction], arguments.internalRequest);
        this._access_check(this.access_level, arguments.internalRequest);
    }

    function do(fuseAction, internalRequest=False) {
        var circuit = false;
        var result = '';
        if (listlen(arguments.fuseAction, '.') gt 1) {
            circuit = this.get_method_circuit(arguments.fuseAction);
            arguments.fuseAction = listLast(arguments.fuseAction, '.');

        } else {
            circuit = this.current_circuit;
        }
        circuit.fb.access_check(arguments.fuseAction, internalRequest);
        savecontent variable='result' {
            circuit[arguments.fuseAction]();
        }
        return result;
    }

    function circuit_include(template)  {
        //can't believe this works
        var result = '';
        savecontent variable='result' {
            include arguments.template;
        }
        return result;
    }

    function include(template, saveContent=False) {
        var result = '';
        arguments.template = '/' & this.current_circuit.filePath & '/' & arguments.template;
        this.current_circuit.circuit_include = this.circuit_include;
        return this.current_circuit.circuit_include(template);
    }

    function handleRequest(method) {
        var i = 0;
        var ex2 = {};
        if (not structKeyExists(arguments, 'method')) {
            arguments.method = url.method;
        }
        try {
            //TODO: reset here
            //content(reset=True);
            writeOutput(this.do(method));
            return true;
        } catch ('expression' ex) {
            if (refindnocase('^invalid component definition', ex.message) gt 0) {
                dump(ex); abort;
                return false;
            } else {
                rethrow;
            }
        }
    }

    function defaults(structure, defaults) {
        var keys = structKeyArray(defaults);
        var i = 0;

        for (i = 1; i lte arraylen(keys); ++i) {
            if (not structKeyExists(structure, keys[i])) {
                structure[keys[i]] = defaults[keys[i]];
            }
        }
        return structure;
    }
}
