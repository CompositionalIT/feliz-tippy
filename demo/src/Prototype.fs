module Prototype

// This file can be used to prototype and test the binding

open Feliz
open Feliz.Tippy

[<ReactComponent>]
let Component () =
    Tippy.create [
        Tippy.plugins [|
            Plugins.followCursor
            Plugins.animateFill
            Plugins.inlinePositioning |]
        Tippy.content ( Html.p "hello world  xxxxxx yyyyy zzzzzz" )
        prop.children [
            Html.h1 "feliz_tippy"
        ]
        Tippy.placement Auto
        //Tippy.arrow false
        //Tippy.arrow "<svg width=\"100\" height=\"100\"><circle cx=\"50\" cy=\"50\" r=\"40\" stroke=\"green\" stroke-width=\"4\" fill=\"yellow\" /></svg>"
        //Tippy.delay (Milliseconds 500)
        //Tippy.animation (Animation.Scale Extreme)
        //Tippy.inertia
        //Tippy.duration (show = Milliseconds 1000, hide = (Milliseconds 1000))
        Tippy.animateFill
        //Tippy.followCursor true
        //Tippy.disabled
        //Tippy.getReferenceClientRect
        //    (fun () ->
        //        { Width = 10
        //          Height = 10
        //          Left = 0
        //          Right = 0
        //          Top = 0
        //          Bottom = 0 })
        Tippy.trigger [MouseEnter; Click]
        //Tippy.hideOnClick DontHide
        //Tippy.inlinePositioning
        Tippy.interactive
        //Tippy.interactiveBorder 100
        //Tippy.offset { Skidding = 200; Distance = 200 }
        //Tippy.maxWidth 100
        //Tippy.touch (LongPress (TimeSpan.FromSeconds 500.))
        //Tippy.zIndex 10
    ]
