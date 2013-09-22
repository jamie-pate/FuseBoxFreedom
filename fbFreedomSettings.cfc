component hint="see fbFreedom.cfc http://www.github.com/jamie-pate/FuseBoxFreedom" output="False" {
    this.classpath = 'cfcs.';
    //Add class to component aliases. If the name is the same it will automatically work
    this.classes = {
        abc='xyz'
    };

    this.circuitpath = '';
    //add circuit to circuit.cfc aliases. Should work automatically for dotted paths.
    this.circuits = {
        checkout='store.checkout2'
    };
}
