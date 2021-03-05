module Index

open Elmish

open Fable.Core.JsInterop

open Fable.React
open Fable.React.Props

open Feliz.Tippy
open Feliz
open System


type Model =
    { Hello: string }

type Msg =
    | GotHello of string

let init() =
    let model : Model =
        { Hello = "" }
    model, Cmd.none

let update msg model =
    match msg with
    | GotHello hello ->
        { model with Hello = hello }, Cmd.none

let view model dispatch =
    div [ Style [ TextAlign TextAlignOptions.Center; Padding 40 ] ] [
        div [] [
            img [ Src "favicon.png" ]
            Tippy.create [
                Tippy.plugins [| 
                    Plugins.followCursor
                    Plugins.animateFill
                    Plugins.inlinePositioning |]
                Tippy.content (em [] [ str "hello world  xxxxxx yyyyy zzzzzz" ])
                Tippy.placement Auto
                //Tippy.arrow false
                //Tippy.arrow "<svg width=\"100\" height=\"100\"><circle cx=\"50\" cy=\"50\" r=\"40\" stroke=\"green\" stroke-width=\"4\" fill=\"yellow\" /></svg>"
                //Tippy.delay (Milliseconds 500)
                //Tippy.animation (Animation.Scale Extreme)
                //Tippy.inertia
                //Tippy.duration (show = Milliseconds 1000, hide = (Milliseconds 1000))
                //Tippy.animateFill
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
                //Tippy.trigger [MouseEnter; Click]
                //Tippy.hideOnClick DontHide
                //Tippy.inlinePositioning
                //Tippy.interactive 
                //Tippy.interactiveBorder 100
                //Tippy.offset { Skidding = 200; Distance = 200 }
                //Tippy.maxWidth 100
                //Tippy.touch (LongPress (TimeSpan.FromSeconds 500.))
                //Tippy.zIndex 10
                prop.children [h1 [] [ str "feliz_tippy" ]]
            ]
        ]
    ]
