module Demo

// Everything after this comment will be included
// It is controlled via the line number in the `sample_file` front-matter property

open Feliz
open Feliz.Bulma
open Feliz.Tippy

[<ReactComponent>]
let Component () =
    Tippy.create [
        Tippy.plugins [|
            Plugins.followCursor
            Plugins.animateFill
            Plugins.inlinePositioning
        |]

        Tippy.content (
            Html.p "Hello from tippy :)"
        )

        prop.children [
            Bulma.button.a [
                prop.text "HOVER OVER ME"
            ]
        ]

        Tippy.placement Auto
        Tippy.animateFill
        Tippy.interactive
    ]
