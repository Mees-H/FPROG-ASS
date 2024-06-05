module Rommulbad.Data.Candidate

open Rommulbad.Application.Candidate
open Rommulbad.Model.Model
open Rommulbad.Data.Database
open Rommulbad.Data.Store

let candidatePersistance = 
    { new ICandidateDataAccess with
        member this.AddCandidate (name: string) (gId: string) (dpl: string) = 
            let result = InMemoryDatabase.insert (name) (name, gId, dpl) store.candidates
            match result with
            | Error _ -> None
            | Ok _-> Some { Candidate.Name = name
                            GuardianId = gId
                            Diploma = dpl }

        member this.GetCandidates () = 
            InMemoryDatabase.all store.candidates
            |> Seq.map (fun (name, gId, dpl) ->
                { Candidate.Name = name
                  GuardianId = gId
                  Diploma = dpl })
            |> Seq.toList

        member this.GetCandidate (name: string) = 
            let candidate = InMemoryDatabase.lookup name store.candidates
            match candidate with
            | None -> None
            | Some(name, gId, dpl) -> Some { Candidate.Name = name
                                             GuardianId = gId
                                             Diploma = dpl }
    }