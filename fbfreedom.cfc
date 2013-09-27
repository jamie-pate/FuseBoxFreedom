component hint="http://www.github.com/jamie-pate/FuseBoxFreedom" output="false"{
    this.acl = {};
    this.access_level = "public";

    function init(circuit_or_scope=false, circuit_scope=false) {
        if (not isObject(arguments.circuit_or_scope) and isStruct(arguments.circuit_or_scope)) {
            arguments.scope = arguments.circuit_or_scope;
            arguments.circuit_or_scope = false;
            this.circuit_name = '<master circuit>';
            this.init_scope(arguments.scope);
        }
        if (isObject(arguments.circuit_or_scope)) {
            this.current_circuit = arguments.circuit_or_scope;
            this.circuit_scope = arguments.circuit_scope;
            this.copy_scope({empty='empty'}, this.circuit_scope);
            arguments.scope = false;
            this.circuit_name = '<pending>';
            this.is_master = false;
            //set later during get_method_circuit
        } else {
            //main instance has no circuit
            this.circuit_scope = false;
            this.current_circuit = false;
            this.is_master = true;
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
        var cscope = this.circuit_scope;
        if (listLen(arguments.value, '.') eq 1) {
            arguments.value = this.circuit_name & '.' & arguments.value;
        }
        this.structSet(cscope, 'xfa', {}, false);
        this.structSet(cscope.xfa, name, value, overwrite);
    }

    function instantiate(class) {
        var classes = this.settings.classes;
        var result = false;
        this.structSet(classes, arguments.class, arguments.class, false);
        result = createobject('component', this.settings.classpath & classes[arguments.class]);
        if (structkeyexists(result, 'init')) {
            result = result.init();
        }
        return result;
    }

    /**
    * get the circuit for the requested fuseaction.Returns the current circuit if no path is specified.
    * @param {string} fuseAction Action to execute 'fuseaction' for local fuseaction, 'circuit.fuseaction'
    *   to look up a different circuit.
    * @return {{input_scope: struct|false, output_scope: struct|false, circuit: component|false}}
    * component and scopes. Scopes can be false if not applicable. Can return false if circuit is blocked
    * TODO: check for current_circuit.fuseAction and short circuit.
    * TODO: not sure if blocking a circuit this way makes sense, should probably throw an exception to
    *   cause handleResponse to return false so fb can process the request instead.
    */
    function get_method_circuit(fuseAction) {
        var path = listDeleteAt(arguments.fuseAction,
                                listlen(arguments.fuseAction, '.'),
                                '.');
        var circuit_name = path;
        var result = false;
        var scope = this.scope;
        var input_scope = scope;
        var output_scope = scope;
        if (structKeyExists(this.settings.circuits, path)) {
            path = this.settings.circuits[path];
            //probably false to block this circuit
            if (isBoolean(path)) {
                return path;
            }
        }
        if (not this.is_master) {
            scope = this.circuit_scope;
        }
        if (structKeyExists(this.circuits, path)) {
            result = this.circuits[path];
            input_scope = scope;
            output_scope = scope;
            if (result.fb.circuit_name eq this.circuit_name) {
                input_scope = false;
                output_scope = false;
            }
        } else {
            result = createObject('component', this.settings.circuitPath & path & '.circuit');
            result.filePath = replace(this.settings.circuitPath & path, '.', '/', 'all');
            result.path = this.settings.circuitPath & path;
            result.fb.scope = this.scope;
            result.fb.circuit_name = circuit_name;
            result.fb.circuits = this.circuits;
            this.circuits[path] = result;
            input_scope = false;
            output_scope = scope;
            this.copy_scope(scope, result.fb.circuit_scope);
            result.init();
        }
        return {input=input_scope, output=output_scope, circuit=result, debug=debug};
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
        var scopes = false;
        if (listlen(arguments.fuseAction, '.') gt 1) {
            scopes = this.get_method_circuit(arguments.fuseAction);
            if (isBoolean(scopes)) {
                return scopes;
            }
            circuit = scopes.circuit;
            arguments.fuseAction = listLast(arguments.fuseAction, '.');

        } else {
            circuit = this.current_circuit;
            scopes = {input=false, output=false}
        }
        circuit.fb.access_check(arguments.fuseAction, internalRequest);
        if (isStruct(scopes.input)) {
            this.copy_scope(scopes.input, circuit.fb.circuit_scope);
            circuit.fb.circuit_scope.copied;
        }
        savecontent variable='result' {
            circuit[arguments.fuseAction]();
        }
        if (isStruct(scopes.output)) {
            //FIXME: figure out why is this happening and stop it (unit tests)
            if (not this.scope_equal(circuit.fb.circuit_scope, scopes.output)) {
                this.copy_scope(circuit.fb.circuit_scope, scopes.output);
            }
        }
        return result;
    }

    function debug(value) {
        return;
        if(structkeyexists(this, 'scope') and isstruct(this.scope)) {
            arrayappend(this.scope.debug, value);
        }
    }

    function circuit_include(template)  {
        var result = '';
        savecontent variable='result' {
            include arguments.template;
        }
        return result;
    }

    function include(template, saveContent=False) {
        var result = '';
        arguments.template = '/' & this.current_circuit.filePath & '/' & arguments.template;
        this.current_circuit.__circuit_include = this.circuit_include;
        //can't believe this works
        //FIXME: there's got to be a better way
        return this.current_circuit.__circuit_include(template);
    }

    function handleRequest(method) {
        var i = 0;
        var ex2 = {};
        var result = '';
        if (not structKeyExists(arguments, 'method')) {
            if (structKeyExists(url, 'method')) {
                arguments.method = url.method;
            } else {

                arguments.method = this.settings.defaultFuseAction;
            }
        }
        if (structkeyexists(this.settings, 'blockedFuseActions') and
                refindnocase(this.settings.blockedFuseActions, method) gt 0) {
            return false;
        }
        try {
            //TODO: reset here?
            //content(reset=True);
            result = this.do(method);
            if (isBoolean(result)) {
                return result;
            }
            writeOutput(result);
            return true;
        } catch ('expression' ex) {
            if (refindnocase('^invalid component definition', ex.message) gt 0) {
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
            this.structSet(structure, keys[i], defaults[keys[i]], false);
        }
        return structure;
    }

    function scope_equal(a, b) {
        //FIXME: find a better way to identity compare!
        structDelete(arguments.b, '___is_scope');
        arguments.a.___is_scope = true;
        return (structKeyExists(arguments.b, '___is_scope'));
    }

    function copy_scope(src, dest) {
        if (scope_equal(arguments.src, arguments.dest)) {
                //tried to copy scope to itself
                throw "omg you killed kenny";
        }
        if (isStruct(arguments.src) and isstruct(arguments.dest)) {
            if (not structKeyExists(arguments.src, 'copied')) {
                arguments.src.copied = 0;
            }
            arguments.src.copied = arguments.src.copied + 1;
            structClear(arguments.dest);
            result = this.defaults(arguments.dest, arguments.src);
        }
    }
}
