component hint="http://www.github.com/jamie-pate/FuseBoxFreedom" output="False"{
    function init(circuit_or_scope=false, circuit_scope=false) {
        if (not isObject(arguments.circuit_or_scope) and isStruct(arguments.circuit_or_scope)) {
            arguments.scope = arguments.circuit_or_scope;
            arguments.circuit_or_scope = false;
            this.current_circuit = false;
            this.init_scope(arguments.scope);
        }
        if (isObject(arguments.circuit_or_scope)) {
            this.current_circuit = arguments.circuit_or_scope;
            this.current_circuit.circuit_scope = arguments.circuit_scope;
            arguments.scope = false;
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


    function set_xfa(name, value, overwrite=true) {
        if (not structKeyExists(this.scope, "xfa")) {
            this.scope.xfa = {};
        }
        if (overwrite or not structkeyexists(this.scope.xfa, name)) {
            this.scope.xfa[name] = value;
        }
    }

    function instantiate(class) {
        var classes = this.settings.classes;
        if (not structkeyexists(classes, arguments.class)) {
            classes[arguments.class] = arguments.class;
        }
        return createobject('component', this.settings.classpath & classes[arguments.class]);
    }


    function copy_scope(src, dest) {
        if (isStruct(src) and isstruct(dest)) {
            structClear(dest);
            return this.defaults(dest, src);
        }
    }

    function get_method_circuit(fuseAction) {
        var path = listDeleteAt(arguments.fuseAction,listlen(arguments.fuseAction, '.'), '.');
        var result = false;
        if (structKeyExists(this.settings.circuits, path)) {
            path = this.settings.circuits[path];
        }
        if (structKeyExists(this.circuits, path)) {
            return this.circuits[path];
        } else {
            result = createObject('component', this.settings.circuitPath & path & '.circuit');
            result.filePath = replace(this.settings.circuitPath & path, '.', '/', 'all');
            result.path = this.settings.circuitPath & path;
            //propagate global variable scope
            result.scope = this.scope;
            result.fb.scope = this.scope;
            this.copy_scope(this.scope, result.circuit_scope);
            result.init();
            this.settings.circuits[path] = result;
            return result;
        }
    }

    function do(fuseAction, saveContent=False) {
        var circuit = false;
        var result = '';
        if (listlen(arguments.fuseAction, '.') gt 1) {
            circuit = this.get_method_circuit(arguments.fuseAction);
            arguments.fuseAction = listLast(arguments.fuseAction, '.');

        } else {
            circuit = this.current_circuit;
        }
        if (arguments.saveContent) {
            saveContent variable=result {
                circuit[arguments.fuseAction]();
            }
        } else {
            result = circuit[arguments.fuseaction]();
        }
        return result;
    }

    function circuit_include(template) {
        //can't believe this works
        include arguments.template;
    }

    function include(template, saveContent=False) {
        var result = '';
        arguments.template = '/' & this.current_circuit.filePath & '/' & arguments.template;
        this.current_circuit.circuit_include = this.circuit_include;
        if (saveContent) {
            savecontent variable="result" {
                this.current_circuit.circuit_include(template)
            }
            result;
        } else {
            this.current_circuit.circuit_include(arguments.template);
        }
        return result;
    }

    function handleRequest(method) {
        var i = 0;
        if (not structKeyExists(arguments, 'method')) {
            arguments.method = url.method;
        }
        return this.do(method);
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