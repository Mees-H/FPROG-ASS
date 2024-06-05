module Rommulbad.Data.Guardian

open Rommulbad.Application.Guardian
open Rommulbad.Model.Model
open Rommulbad.Data.Database
open Rommulbad.Data.Store

let guardianPersistance = 
    { new IGuardianDataAccess with
        member this.AddGuardian (id: string) (name: string) = 
            let result = InMemoryDatabase.insert (id) (id, name) store.guardians
            match result with
            | Error _ -> None
            | Ok _-> Some { Guardian.Id = id
                            Name = name }
    }
