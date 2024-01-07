module Main

open Feliz
open App
open Browser.Dom
open Fable.Core.JsInterop

importSideEffects "./styles/global.scss"

let root = ReactDOM.createRoot(document.getElementById "feliz-app")

root.render(
    Components.Documentation()
)