module Rommulbad.Model.Validation

open System.Text.RegularExpressions

let matches (re: Regex) invalid (s: string) = 
    if re.IsMatch s then Ok s else Error invalid

let shallowOk (diploma: string) =
    match diploma with
    | "A" -> true
    | _ -> false

let minMinutes (diploma: string) =
    match diploma with
    | "A" -> 1
    | "B" -> 10
    | _ -> 15
