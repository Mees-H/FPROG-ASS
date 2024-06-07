module Rommulbad.Service.Candidate

open Giraffe
open Thoth.Json.Net
open Thoth.Json.Giraffe
open Microsoft.AspNetCore.Http
open Rommulbad.Model.Common

open Rommulbad.Application.Candidate
open Rommulbad.Application.Session

let addCandidate : HttpHandler =
    fun next ctx ->
        task {
            let! candidate = ThothSerializer.ReadBody ctx Serialization.Candidate.decode

            match candidate with
            | Error message -> 
                return! RequestErrors.badRequest (text message) earlyReturn ctx
            | Ok { Name = name
                   GuardianId = guardianId
                   Diploma = diploma } ->
                let store = ctx.GetService<ICandidateDataAccess>()

                let candidate = addCandidate store name guardianId diploma
                match candidate with
                | None -> 
                    return! RequestErrors.badRequest (text "Something went wrong") earlyReturn ctx
                | Some candidate ->
                    return! ThothSerializer.RespondJson candidate Serialization.Candidate.encode next ctx
        }

let getCandidates: HttpHandler =
    fun next ctx ->
        task {
            let store = ctx.GetService<ICandidateDataAccess>()

            let candidates = getCandidates store

            return! ThothSerializer.RespondJsonSeq candidates Serialization.Candidate.encode next ctx
        }

let getCandidate (name: string) : HttpHandler =
    fun next ctx ->
        task {
            match Name.make name with
            | Error message -> 
                return! RequestErrors.badRequest (text message) earlyReturn ctx
            | Ok name ->
                let store = ctx.GetService<ICandidateDataAccess>()
                let candidate = getCandidate store (Name.value name)
                match candidate with
                | None -> 
                    return! RequestErrors.notFound (text "Candidate doesn't exist") earlyReturn ctx
                | Some candidate ->
                    return! ThothSerializer.RespondJson candidate Serialization.Candidate.encode next ctx
        }

let getQualifiedCandidates (diploma: string) : HttpHandler =
    fun next ctx ->
        task {
            match Diploma.make diploma with
            | Error message -> 
                return! RequestErrors.badRequest (text message) next ctx
            | Ok (shallowOk, minMinutes) ->
                let sessionStore = ctx.GetService<ISessionDataAccess>()
                let candidateStore = ctx.GetService<ICandidateDataAccess>()

                let candidates = Rommulbad.Application.Candidate.getCandidates candidateStore
                let names = candidates |> List.map (fun candidate -> candidate.Name)

                let minutesPerCandidate = getTotalEligibleMinutesAllCandidatesGrouped sessionStore names shallowOk minMinutes
                let qualified = minutesPerCandidate |> List.map (fun (names, minutes) -> Qualified.make diploma names minutes)
                let qualifiedNames = 
                    qualified 
                    |> List.map (function
                        | Ok name -> name
                        | Error message -> message)
                    |> List.filter (fun name -> name <> "")

                return! ThothSerializer.RespondJsonSeq qualifiedNames Encode.string next ctx
        }

let putCandidateDiploma (name: string, diploma: string) : HttpHandler =
    fun next ctx ->
        task {
            let store = ctx.GetService<ICandidateDataAccess>()
            let candidate = putCandidateDiploma store name diploma
            match candidate with
            | None -> 
                return! RequestErrors.notFound (text "Candidate doesn't exist") earlyReturn ctx
            | Some candidate ->
                return! ThothSerializer.RespondJson candidate Serialization.Candidate.encode next ctx
        }

let candidateRoutes: HttpHandler =
    choose
        [ POST >=> route "/candidate" >=> addCandidate
          GET >=> route "/candidate" >=> getCandidates
          GET >=> routef "/candidate/%s" getCandidate
          GET >=> routef "/candidate/qualified/%s" getQualifiedCandidates
          PUT >=> routef "/candidate/%s/diploma/%s" putCandidateDiploma ]