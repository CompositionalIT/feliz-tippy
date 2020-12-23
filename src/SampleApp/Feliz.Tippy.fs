module Feliz.Tippy

open Fable.Core.JsInterop
open Fable.Core

let tippy : obj = importDefault "@tippyjs/react"

importAll "tippy.js/dist/tippy.css"

[<Erase>]
type Tippy =
    
    static member inline content (reactElement:ReactElement) =
        prop.custom("content", reactElement)

    static member inline create (props : IReactProperty seq) = 
        let (children, others) = 
            props
            |> unbox<(string*obj) seq>
            |> Seq.toArray
            |> Array.partition (fun (key,_) -> key = "children")
            
        let children = children |> Array.tryLast |> Option.map snd |> Option.toObj

        Interop.reactApi.createElement (tippy, createObj !!others, !!children)

