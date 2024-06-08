module Rommulbad.Model.Common

open System.Text.RegularExpressions

type Name = private | Name of string

let (|Name|) (Name name) = name

[<RequireQualifiedAccess>]
module Name =

    let private validName = Regex "^[a-zA-Z]+( [a-zA-Z]+)*$"

    let make rawName =
        rawName
        |> Validation.matches validName "Candidates' and Guardians' names consist of words separated by a singlespace. Names do not end with a space"
        |> Result.map Name

    let value (Name name) = name

type Diploma = private | Diploma of string

let (|Diploma|) (Diploma diploma) = diploma

[<RequireQualifiedAccess>]
module Diploma =

    let make rawDiploma =
        let shallowOk = Validation.shallowOk rawDiploma
        let minMinutes = Validation.minMinutes rawDiploma
        Ok (shallowOk, minMinutes)

    let value (Diploma diploma) = diploma

type Qualified = private | Qualified of List<string>

let (|Qualified|) (Qualified qualified) = qualified

[<RequireQualifiedAccess>]
module Qualified =

    let make diploma name minutes =
        let qualified = Validation.qualified (diploma) (name) (minutes)
        match qualified with
        | "" -> Error $"{name} is not qualified for diploma {diploma}"
        | _ -> Ok (qualified)


    let value (Qualified qualified) = qualified

type GuardianId = private | GuardianId of string

let (|GuardianId|) (GuardianId guardianId) = guardianId

[<RequireQualifiedAccess>]
module GuardianId =

    let private validGuardianId = Regex "^\d{3}-[A-Z]{4}$"

    let make rawGuardianId =
        rawGuardianId
        |> Validation.matches validGuardianId "GuardianId must start with three digits, followed by a dash and end with four capital letters"
        |> Result.map GuardianId

    let value (GuardianId guardianId) = guardianId

type DupeCandidate = private | DupeCandidate of string

let (|DupeCandidate|) (DupeCandidate candidate) = candidate

[<RequireQualifiedAccess>]
module DupeCandidate =

    let make name names guardianId guardianIds =
        let dupe = Validation.checkDupe name names guardianId guardianIds
        match dupe with
        | Error message -> Error message
        | Ok _ -> Ok (DupeCandidate name)

    let value (DupeCandidate candidate) = candidate

type Minutes = private | Minutes of int

let (|Minutes|) (Minutes minutes) = minutes

[<RequireQualifiedAccess>]
module Minutes =

    let make minutes =
        let valid = Validation.checkValidMinutes minutes
        match valid with
        | Error message -> Error message
        | Ok _ -> Ok (Minutes minutes)

    let value (Minutes minutes) = minutes