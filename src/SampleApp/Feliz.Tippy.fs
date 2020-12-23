module Feliz.Tippy

open Fable.Core.JsInterop

let tippy : obj = importDefault "@tippyjs/react"

importAll "tippy.js/dist/tippy.css"

let createTippy props children =
    Interop.reactApi.createElement (tippy, createObj props, children)

