module Rommulbad.Model.Validation

open System.Text.RegularExpressions
open Rommulbad.Model.Model

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

let qualified (diploma: string) (name: string) (minutes: int) =
    match minutes with
    | minutes when minutes >= 180 && (diploma = "C") -> name
    | minutes when minutes >= 150 && (diploma = "B") -> name
    | minutes when minutes >= 120 && (diploma = "A") -> name
    | _ -> ""

let checkDupe (name: string) (names: List<string>) (guardianId: string) (guardianIds: List<string>) =
    match names with
    | names when List.contains name names -> Error $"{name} already exists"
    | _ -> Ok name

let checkValidMinutes (minutes: int) =
    match minutes with
    | minutes when minutes < 0 -> Error "Number of minutes cannot be negative"
    | minutes when minutes > 30 -> Error "Number of minutes cannot be larger than 30"
    | _ -> Ok minutes
