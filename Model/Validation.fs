module Rommulbad.Model.Validation

open System.Text.RegularExpressions

let matches (re : Regex) invalid (s: string) = if re.IsMatch s then Ok s else Error invalid