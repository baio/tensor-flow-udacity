 let takeEveryNth n lst = 
  lst |> List.mapi (fun i el -> el, i)              // Add index to element
      |> List.filter (fun (el, i) -> i % n = 0) // Take every nth element
      |> List.map fst                               // Drop index from the result

