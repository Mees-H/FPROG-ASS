module Rommulbad.Service.Guardian

open Giraffe
open Thoth.Json.Net
open Thoth.Json.Giraffe

open Rommulbad.Application.Guardian
open Rommulbad.Model.Common
open Rommulbad.Model.Model

let addGuardian : HttpHandler =
    fun next ctx ->
        task {
            let! guardian = ThothSerializer.ReadBody ctx Serialization.Guardian.decode

            match guardian with
            | Error message -> 
                return! RequestErrors.badRequest (text message) next ctx
            | Ok { Id = id
                   Name = name } ->
                let store = ctx.GetService<IGuardianDataAccess>()

                let guardian = addGuardian store id name
                match guardian with
                | None -> 
                    return! RequestErrors.badRequest (text "Something went wrong") earlyReturn ctx
                | Some guardian ->
                    return! ThothSerializer.RespondJson guardian Serialization.Guardian.encode next ctx
        }

let guardianRoutes: HttpHandler =
    choose
        [ POST >=> route "/guardian" >=> addGuardian ]