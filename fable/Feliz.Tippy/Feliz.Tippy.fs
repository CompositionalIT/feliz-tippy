module Feliz.Tippy

open Fable.Core.JsInterop
open Fable.Core
open System

let tippy : obj = importDefault "@tippyjs/react"

module Plugins =
    let followCursor : obj = import "followCursor" "tippy.js"
    let animateFill : obj = import "animateFill" "tippy.js"

importAll "tippy.js/dist/tippy.css"
importAll "tippy.js/dist/backdrop.css"
importAll "tippy.js/animations/scale.css"
importAll "tippy.js/animations/scale-subtle.css"
importAll "tippy.js/animations/scale-extreme.css"
importAll "tippy.js/animations/perspective.css"
importAll "tippy.js/animations/perspective-subtle.css"
importAll "tippy.js/animations/perspective-extreme.css"
importAll "tippy.js/animations/shift-away.css"
importAll "tippy.js/animations/shift-away-subtle.css"
importAll "tippy.js/animations/shift-away-extreme.css"
importAll "tippy.js/animations/shift-toward.css"
importAll "tippy.js/animations/shift-toward-subtle.css"
importAll "tippy.js/animations/shift-toward-extreme.css"

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

type Milliseconds = 
    | Milliseconds of int
    member this.Value =
        match this with
        | Milliseconds i -> i

type AnimationVariation =
    | Normal
    | Subtle
    | Extreme

    member this.Value =
        match this with
        | Normal -> String.Empty
        | Subtle -> "-subtle"
        | Extreme ->  "-extreme"

type Animation = 
    | Fade
    | Scale of AnimationVariation
    | Perspective of AnimationVariation
    | ShiftAway of AnimationVariation
    | ShiftToward of AnimationVariation

    member this.Value =
        match this with
        | Fade -> "fade"
        | Scale v ->  "scale" + v.Value
        | Perspective v -> "perspective" + v.Value
        | ShiftAway v -> "shift-away" + v.Value 
        | ShiftToward v -> "shift-toward" + v.Value

[<Erase>]
type Tippy =
    
    static member inline content (reactElement:ReactElement) =
        prop.custom("content", reactElement)

    static member inline content (content : string) =
        prop.custom("content", content)

    static member inline disabled  =
        prop.custom("disabled", true)

    static member inline placement (position : Placement) =
        prop.custom("placement", position.Value)

    static member inline delay (Milliseconds delay) =
        prop.custom("delay", delay)

    // Doesn't work - https://github.com/atomiks/tippyjs-react/issues/94
    //static member inline allowHTML (allow : bool) =
    //    prop.custom("allowHTML", allow)

    /// Requires you to pass Tippy.Plugins.animateFill to Tippy.plugins
    static member inline animateFill =
        prop.custom("animateFill", true)

    /// Requires CSS imports - currently done by default in the binding, may move to Client responsibility.
    static member inline animation  (animation : Animation) =
        prop.custom("animation", animation.Value)

    static member inline inertia  =
        prop.custom("inertia", true)

    // Not working, needs a DOM ref I think - https://atomiks.github.io/tippyjs/v6/all-props/#appendto
    //static member inline appendTo (reactElement : ReactElement) =
    //    prop.custom("appendTo", reactElement)

    static member inline arrow (enabled : bool) =
        prop.custom("arrow", enabled)

    /// A string is parsed as .innerHTML. Don't pass unknown user data to this prop.
    static member inline arrow (svg : string) =
        prop.custom("arrow", svg)

    static member inline duration (?show : Milliseconds, ?hide : Milliseconds) =
        prop.custom(
            "duration", 
            [| show |> Option.map (fun m -> m.Value)
               hide |> Option.map (fun m -> m.Value) |])

    /// Requires you to pass Tippy.Plugins.followCursor to Tippy.plugins
    static member inline followCursor (follow : bool) =
        prop.custom("followCursor", follow)

    static member inline plugins (plugins : obj[]) =
        prop.custom("plugins", plugins)

    


    static member inline create (props : IReactProperty seq) = 
        let elements = splitChildProps props
        Interop.reactApi.createElement(tippy, createObj !!elements.Props, !!elements.Children)

