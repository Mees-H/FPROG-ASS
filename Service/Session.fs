module Rommulbad.Service.Session

open Giraffe
open Thoth.Json.Net
open Thoth.Json.Giraffe

open Rommulbad.Application.Session
open Rommulbad.Model.Common
open Rommulbad.Model.Model

let addSession (name: string) : HttpHandler =
    fun next ctx ->
        task {
            let! session = ThothSerializer.ReadBody ctx Serialization.Session.decode

            match session with
            | Error message -> 
                return! RequestErrors.badRequest (text message) next ctx
            | Ok { Deep = deep
                   Date = date
                   Minutes = minutes } ->
                match Minutes.make minutes with
                | Error message -> 
                    return! RequestErrors.badRequest (text message) next ctx
                | Ok _ ->
                    let store = ctx.GetService<ISessionDataAccess>()

                    let session = addSession store name deep date minutes
                    match session with
                    | None -> 
                        return! RequestErrors.badRequest (text "Something went wrong") earlyReturn ctx
                    | Some session ->
                        return! ThothSerializer.RespondJson session Serialization.Session.encode next ctx
        }

let getSessions (name: string) : HttpHandler =
    fun next ctx ->
        task {
            let store = ctx.GetService<ISessionDataAccess>()

            let sessions = getSessions store name

            return! ThothSerializer.RespondJsonSeq sessions Serialization.Session.encode next ctx
        }

let getTotalMinutes (name: string) : HttpHandler =
    fun next ctx ->
        task {
            let store = ctx.GetService<ISessionDataAccess>()

            let total = getTotalMinutes store name

            return! ThothSerializer.RespondJson total Encode.int next ctx
        }


let getEligibleSessions (name: string, diploma: string) : HttpHandler =
    fun next ctx ->
        task {
            match Diploma.make diploma with
            | Error message -> 
                return! RequestErrors.badRequest (text message) next ctx
            | Ok (shallowOk, minMinutes) ->
                let store = ctx.GetService<ISessionDataAccess>()
                let sessions = getEligibleSessions store name shallowOk minMinutes                   
                return! ThothSerializer.RespondJsonSeq sessions Serialization.Session.encode next ctx

        }

let getTotalEligibleMinutes (name: string, diploma: string) : HttpHandler =
    fun next ctx ->
        task {
            match Diploma.make diploma with
            | Error message -> 
                return! RequestErrors.badRequest (text message) next ctx
            | Ok (shallowOk, minMinutes) ->
                let store = ctx.GetService<ISessionDataAccess>()
                let total = getTotalEligibleMinutes store name shallowOk minMinutes
                return! ThothSerializer.RespondJson total Encode.int next ctx
        }

let sessionRoutes: HttpHandler =
    choose
        [ POST >=> routef "/candidate/%s/session" addSession
          GET >=> routef "/candidate/%s/session" getSessions
          GET >=> routef "/candidate/%s/session/total" getTotalMinutes
          GET >=> routef "/candidate/%s/session/%s" getEligibleSessions
          GET >=> routef "/candidate/%s/session/%s/total" getTotalEligibleMinutes ]