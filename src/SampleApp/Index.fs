module Index

open Elmish

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

open Fable.React
open Fable.React.Props

let view model dispatch =
    div [ Style [ TextAlign TextAlignOptions.Center; Padding 40 ] ] [
        div [] [
            img [ Src "favicon.png" ]
            Tippy.create [
                Tippy.content (em [] [ str "hello world" ])
                Tippy.placement Auto
                prop.children [h1 [] [ str "feliz_tippy" ]]
            ]
        ]
    ]
