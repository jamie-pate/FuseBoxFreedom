<circuit access="public">
    <prefuseaction>
        <set name="testvar" value="prefuse"/>
        <set name="test_title" value="FuseBox Freedom!"/>
    </prefuseaction>
    <fuseaction name="test_fa">
        <set name="testvar2" value="fuseaction"/>
        <set name="testvar" value="fuseaction" overwrite="false"/>
        <include template="test_fa.cfm" contentvariable="test_result"/>
        <include template="test_fa_result.cfm"/>
    </fuseaction>
</circuit>
