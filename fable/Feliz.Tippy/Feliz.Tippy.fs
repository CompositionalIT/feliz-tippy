module Feliz.Tippy

open Fable.Core.JsInterop
open Fable.Core
open System

let tippy : obj = importDefault "@tippyjs/react"

module Plugins =
    let followCursor : obj = import "followCursor" "tippy.js"
    let animateFill : obj = import "animateFill" "tippy.js"
    let inlinePositioning : obj = import "inlinePositioning" "tippy.js"

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
importAll "tippy.js/themes/light.css"
importAll "tippy.js/themes/light-border.css"
importAll "tippy.js/themes/material.css"
importAll "tippy.js/themes/translucent.css"

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

type Rect =
    { Width : int
      Height : int
      Left : int
      Right : int
      Top : int
      Bottom : int }

type HideOnClick =
    | Hide
    | DontHide
    | Toggle

type Trigger =
    | MouseEnterFocus
    | Click
    | FocusIn
    | MouseEnterClick
    | Manual

    member this.Value =
        match this with
        | MouseEnterFocus -> "mouseenter focus"
        | Click -> "click"
        | FocusIn -> "focusin"
        | MouseEnterClick -> "mouseenter click"
        | Manual -> "manual"

type Offset =
    { Skidding : int
      Distance : int }

//type TransitionTimingFunction =
//    | Ease
//    | EaseIn
//    | EaseOut
//    | EaseInOut
//    | Linear
//    | StepStart
//    | StepEnd
//    | Custom of string
//    member this.Value =
//        match this with
//        | Ease -> "ease"
//        | EaseIn -> "ease-in"
//        | EaseOut -> "ease-out"
//        | EaseInOut -> "ease-in-out"
//        | Linear -> "linear"
//        | StepStart -> "step-start"
//        | StepEnd -> "step-end"
//        | Custom t -> t

//type Transition =
//    { CSSProperty : string
//      Duration : TimeSpan
//      TimingFunction : TransitionTimingFunction
//      Delay : TimeSpan }
//    member this.Value = 
//        sprintf "%s %fs %s %fs" this.CSSProperty this.Duration.TotalSeconds this.TimingFunction.Value this.Delay.TotalSeconds

type Theme =
    | Light
    | LightBorder
    | Material
    | Translucent
    | Custom of string
    member this.Value = 
        match this with
        | Light -> "light"
        | LightBorder -> "light-border"
        | Material -> "material"
        | Translucent -> "translucent"
        | Custom theme -> theme

type TouchBehaviour =
    | On
    | Off
    | Hold
    | LongPress of TimeSpan

[<Erase>]
type Tippy =
    
    /// The content of the tippy.
    static member inline content (reactElement:ReactElement) =
        prop.custom("content", reactElement)

    /// The content of the tippy.
    static member inline content (content : string) =
        prop.custom("content", content)

    static member inline disabled  =
        prop.custom("disabled", true)

    /// The preferred placement of the tippy.
    static member inline placement (position : Placement) =
        prop.custom("placement", position.Value)

    /// Delay in ms once a trigger event is fired before a tippy shows or hides.
    static member inline delay (Milliseconds delay) =
        prop.custom("delay", delay)

    // Doesn't work - https://github.com/atomiks/tippyjs-react/issues/94
    //static member inline allowHTML (allow : bool) =
    //    prop.custom("allowHTML", allow)

    /// Determines if the background fill color of the tippy should be animated.
    /// Requires you to pass Tippy.Plugins.animateFill to Tippy.plugins
    static member inline animateFill =
        prop.custom("animateFill", true)

    /// The type of transition animation.
    /// Requires CSS imports - currently done by default in the binding, may move to Client responsibility.
    static member inline animation  (animation : Animation) =
        prop.custom("animation", animation.Value)

    /// Adds an elastic inertial effect to the tippy, which is a limited CSS-only way to mimic spring physics.
    static member inline inertia  =
        prop.custom("inertia", true)

    // Not working, needs a DOM ref - https://atomiks.github.io/tippyjs/v6/all-props/#appendto
    //static member inline appendTo (reactElement : ReactElement) =
    //    prop.custom("appendTo", reactElement)

    /// Determines if the tippy has an arrow.
    static member inline arrow (enabled : bool) =
        prop.custom("arrow", enabled)

    /// Determines if the tippy has an arrow.
    /// A string is parsed as .innerHTML. Don't pass unknown user data to this prop.
    static member inline arrow (svg : string) =
        prop.custom("arrow", svg)

    /// Duration in ms of the transition animation.
    static member inline duration (?show : Milliseconds, ?hide : Milliseconds) =
        prop.custom(
            "duration", 
            [| show |> Option.map (fun m -> m.Value)
               hide |> Option.map (fun m -> m.Value) |])

    /// Requires you to pass Tippy.Plugins.followCursor to Tippy.plugins
    static member inline followCursor (follow : bool) =
        prop.custom("followCursor", follow)

    /// Plugins to use
    static member inline plugins (plugins : obj[]) =
        prop.custom("plugins", plugins)

    /// Used as the positioning reference for the tippy.
    static member inline getReferenceClientRect (getRect : unit -> Rect) =
        prop.custom("getReferenceClientRect", getRect)

    /// Determines if the tippy hides upon clicking the reference or outside of the tippy. 
    /// The behavior can depend upon the trigger events used.
    static member inline hideOnClick (hideOnClick : HideOnClick) =
        match hideOnClick with
        | Hide -> prop.custom("hideOnClick", true)
        | DontHide -> prop.custom("hideOnClick", false)
        | Toggle -> prop.custom("hideOnClick", "trigger")

    /// Determines the events that cause the tippy to show. 
    /// Multiple event names are separated by spaces.
    static member inline trigger (trigger : Trigger) =
        prop.custom("trigger", trigger.Value)

    /// Provides enhanced support for display: inline elements. 
    /// It will choose the most appropriate rect based on the placement.
    /// Requires you to pass Tippy.Plugins.inlinePositioning to Tippy.plugins
    static member inline inlinePositioning   =
        prop.custom("inlinePositioning", true)

    /// Determines if the tippy has interactive content inside of it,
    /// so that it can be hovered over and clicked inside without hiding.
    static member inline interactive   =
        prop.custom("interactive", true)

    // Doesn't seem to work
    //
    /// Determines the size of the invisible border around the tippy that
    /// will prevent it from hiding if the cursor left it.
    static member inline interactiveBorder (size : int)   =
        prop.custom("interactiveBorder", size)

    /// Determines the time in ms to debounce the interactive hide handler
    /// when the cursor leaves the tippy's interactive region.
    /// Offers a temporal (rather than spacial) alternative to interactiveBorder, 
    /// although it can be used in conjunction with it.
    static member inline interactiveDebounce (time : Milliseconds) =
        prop.custom("interactiveDebounce", time.Value)

    /// Specifies the maximum width of the tippy. Useful to prevent it from being
    /// too horizontally wide to read.
    /// Note
    /// This is applied to the .tippy-box (inner element), rather than the root 
    /// positioned popper node. The core CSS applies max-width: calc(100vw - 10px) 
    /// on the root popper node to prevent it from exceeding the viewport width on small screens.
    static member inline maxWidth (?width : int) =
        match width with
        | Some width -> prop.custom("maxWidth", width)
        | None -> prop.custom("maxWidth", "none")

    /// Displaces the tippy from its reference element in pixels (skidding and distance).
    static member inline offset (offset : Offset) =
        prop.custom(
            "offset", 
            [| offset.Skidding
               offset.Distance |])

    // Useful when you have a singleton Tippy instance to define how it transitions from one place to another
    ///// Specifies the transition applied to the root positioned popper node. This describes
    ///// the transition between "moves" (or position updates) of the popper element when it
    ///// e.g. flips or changes target location.
    //static member inline moveTransition (transition : Transition) =
    //    prop.custom("moveTransition", transition.Value)

    static member inline theme (theme : Theme) =
        prop.custom("theme", theme.Value)

    static member inline touch (touchBehaviour : TouchBehaviour) =
        match touchBehaviour with
        | On -> prop.custom("touch", true)
        | Off -> prop.custom("touch", false)
        | Hold -> prop.custom("touch", "hold")
        | LongPress time -> prop.custom("touch", Interop.mkStyle "hold" time.TotalSeconds)

    static member inline create (props : IReactProperty seq) = 
        let elements = splitChildProps props
        Interop.reactApi.createElement(tippy, createObj !!elements.Props, !!elements.Children)

