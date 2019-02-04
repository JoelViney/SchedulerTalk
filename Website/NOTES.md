
# Vue Notes

* properties cannot start with $ or _

directive = <h1 v-text="blah"> <h1>
semantic syntax (mustaches) = {{ blah }}
    Cant be used inside of html attributes
        e.g. alt, src, style

one time binding = <h2 v-once> {{ blah }}</h2>
    any child will only be rendered once.

    html directive
        <h2 v-html="appName"></h2>

    Cant Nest Bindings..

    v-text
    v-html

    v-bind:src="blah" === :src="blah"

    v-bind:style="{color: color }

kerban to camelcase just works.
    font-family becomes fontFamily

    v-bind:style="[blah, blah2]" <-- binding through array

    v-bind:class="[cssBlah, cssBlah2]"


    v-bind:class="headerStyles"
    data: {
        headerStyles: [
            'style1'
            'style2'
        ]
    }

    vbind:class=-"{'headers-style': true}"

JavaScript Expressions
======================
var isLocal = location.host.includes('localhost')



<h2 v-bind="colorStyle:isOnline ? 'red' : 'blue' }"></h2>
