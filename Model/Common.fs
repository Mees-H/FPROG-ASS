module Rommulbad.Model.Common

open System.Text.RegularExpressions

type Name = private | Name of string

let (|Name|) (Name name) = name

[<RequireQualifiedAccess>]
module Name =

    let private validName = Regex "^[a-zA-Z]+$"

    let make rawName =
        rawName
        |> Validation.matches validName "Name must only contain letters"
        |> Result.map Name

    let value (Name name) = name
    