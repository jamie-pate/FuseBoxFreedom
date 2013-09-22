component hint="http://www.github.com/jamie-pate/FuseBoxFreedom" {
    function init(circuit) {
        if (isdefined('arguments.circuit')) {
            this.current_circuit = circuit;
        } else {
            //main instance has no circuit
            this.current_circuit = false;
        }
        this.settings = createobject('component', 'cfcs.fbFreedomSettings');
    }

    function set_xfa(name, value, overwrite=true) {
        if (overwrite or not structkeyexists(xfa, name)) {
            xfa[name] = value;
        }
    }

    function instantiate(class) {
        var classes = this.settings.classes;
        if (not structkeyexists(classes, arguments.class)) {
            classes[arguments.class] = arguments.class;
        }
        return createobject('component', this.settings.classpath & classes[arguments.class]);
    }

    function do(fuseAction, saveContent=False) {
        var circuit = false;
        var result = '';
        if (listlen(arguments.fuseAction, '.') gt 1) {
            circuit = this.fuse_circuit(arguments.fuseAction);
            arguments.fuseAction = listGetLast(arguments.fuseAction);

        } else {
            circuit = this.current_circuit;
        }
        if (arguments.saveContent) {
            saveContent variable=result {
                circuit[arguments.fuseAction]();
            }
            return result;
        } else {
            circuit[arguments.fuseaction]();
        }
    }

    function include(template, saveContent=False) {
        var result = '';
        if (saveContent) {
            savecontent variable="result" {
                include template;
            }
            return result;
        } else {
            include template;
        }
    }

    function handleRequest(method) {
        var url_keys = structKeyArray(url);
        var form_keys = structKeyArray(form);
        var i = 0;
        if (not structKeyExists(arguments, 'method')) {
            arguments.method = url.method;
        }
        if (not isdefined('attributes')) {
            attributes = structNew();
            for (i = 1; i lte arraylen(url_keys); ++i) {
                attributes[i] = url[i];
            }
            for (i = 1; i lte arraylen(form_keys); ++i) {
                attributes[i] = form[i];
            }
        }
        this.do(method);
    }
}