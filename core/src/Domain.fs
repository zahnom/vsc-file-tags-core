namespace vsc_file_tags_fsharp

[<AutoOpen>]
module Domain =

    type Tag = {
        Name: string;
        File: string;
    }