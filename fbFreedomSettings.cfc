<cfcomponent hint="see fbFreedom.cfc http://www.github.com/jamie-pate/FuseBoxFreedom">
<cfscript>
    this.classpath = 'cfcs.';
    //Add class to component mappings. If the name is the same it will automatically work
    this.classes = {};
    //add circuit to circuit.cfc mappings. Should work automatically for dotted paths.
    this.circuits = {
        storeCheckout2='store.checkout2';
    };
</cfscript>
</cfcomponent>