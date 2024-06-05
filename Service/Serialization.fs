module Rommulbad.Service.Serialization

open Thoth.Json.Net
open Rommulbad.Model.Model
open Newtonsoft.Json

module Candidate =
    let encode: Encoder<Candidate> =
        fun candidate ->
            Encode.object
                [ "name", Encode.string candidate.Name
                  "guardian_id", Encode.string candidate.GuardianId
                  "diploma", Encode.string candidate.Diploma ]

    let decode: Decoder<Candidate> =
        Decode.object (fun get ->
            { Name = get.Required.Field "name" Decode.string
              GuardianId = get.Required.Field "guardian_id" Decode.string
              Diploma = get.Required.Field "diploma" Decode.string })

module Session =
    let encode: Encoder<Session> =
        fun session ->
            Encode.object
                [ "deep", Encode.bool session.Deep
                  "date", Encode.datetime session.Date
                  "amount", Encode.int session.Minutes ]

    let decode: Decoder<Session> =
        Decode.object (fun get ->
            { Deep = get.Required.Field "deep" Decode.bool
              Date = get.Required.Field "date" Decode.datetimeUtc
              Minutes = get.Required.Field "amount" Decode.int })

module Guardian =
    let encode: Encoder<Guardian> =
        fun guardian ->
            Encode.object
                [ "id", Encode.string guardian.Id
                  "name", Encode.string guardian.Name ]

    let decode: Decoder<Guardian> =
        Decode.object (fun get ->
            { Id = get.Required.Field "id" Decode.string
              Name = get.Required.Field "name" Decode.string })