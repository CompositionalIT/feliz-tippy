module Index

open Elmish

open Fable.Core.JsInterop

open Fable.React
open Fable.React.Props

open Feliz.Tippy
open Feliz


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
                Tippy.content (em [] [ str "hello world" ])
                Tippy.placement Auto
                //Tippy.arrow false
                //Tippy.arrow "<svg width=\"100\" height=\"100\"><circle cx=\"50\" cy=\"50\" r=\"40\" stroke=\"green\" stroke-width=\"4\" fill=\"yellow\" /></svg>"
                Tippy.delay (Milliseconds 500)
                Tippy.animation (Animation.Perspective Extreme)
                Tippy.duration (show = Milliseconds 1000, hide = (Milliseconds 1000))
                Tippy.plugins [| followCursorPlugin |]
                Tippy.followCursor true
                prop.children [h1 [] [ str "feliz_tippy" ]]
            ]
        ]
    ]
