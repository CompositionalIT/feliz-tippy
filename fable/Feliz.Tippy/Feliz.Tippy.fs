module Feliz.Tippy

open Fable.Core.JsInterop
open Fable.Core

let tippy : obj = importDefault "@tippyjs/react"

importAll "tippy.js/dist/tippy.css"

let splitChildProps props = 
    let (children, props) = 
        props
        |> unbox<(string*obj) seq>
        |> Seq.toArray
        |> Array.partition (fun (key,_) -> key = "children")

    let children = children |> Array.tryLast |> Option.map snd |> Option.toObj

    {| Props = props; Children = children |}

type Placement =
    | Top
    | TopStart
    | TopEnd
    | Right
    | RightStart
    | RightEnd
    | Bottom
    | BottomStart
    | BottomEnd
    | Left
    | LeftStart
    | LeftEnd
    | Auto
    | AutoStart
    | AutoEnd

    member this.Value =
        match this with
        | Top -> "top"
        | TopStart -> "top-start"
        | TopEnd -> "top-end"
        | Right -> "right"
        | RightStart -> "right-start"
        | RightEnd -> "right-end"
        | Bottom -> "bottom"
        | BottomStart -> "bottom-start"
        | BottomEnd -> "bottom-end"
        | Left -> "left"
        | LeftStart -> "left-start"
        | LeftEnd -> "left-end"
        | Auto -> "auto"
        | AutoStart -> "auto-start"
        | AutoEnd -> "auto-end"

[<Erase>]
type Tippy =
    
    static member inline content (reactElement:ReactElement) =
        prop.custom("content", reactElement)

    static member inline placement (position : Placement) =
        prop.custom("placement", position.Value)

    static member inline create (props : IReactProperty seq) = 

        let elements = splitChildProps props

        Interop.reactApi.createElement(tippy, createObj !!elements.Props, !!elements.Children)

