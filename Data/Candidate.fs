module Rommulbad.Data.Candidate

open Rommulbad.Application.Candidate
open Rommulbad.Model.Model
open Rommulbad.Data.Database
open Rommulbad.Data.Store
open Rommulbad.Model.Common

let candidatePersistance = 
    { new ICandidateDataAccess with
        member this.GetCandidates () = 
            InMemoryDatabase.all store.candidates
            |> Seq.map (fun (name, _, gId, dpl) ->
                { Candidate.Name = name
                  GuardianId = gId
                  Diploma = dpl })
            |> Seq.toList

        member this.GetCandidate (name: string) = 
            let candidate = InMemoryDatabase.lookup name store.candidates
            match candidate with
            | None -> None
            | Some(name, _, gId, dpl) -> Some { Candidate.Name = name
                                                GuardianId = gId
                                                Diploma = dpl }
    }