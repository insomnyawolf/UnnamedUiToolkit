# UnnamedUiToolkit

Originally built as part of ``org.duckdns.insomnyawolf.ndmf-tools``

After a lot of shotcomings and issues i decided to rewrite it (once again ;-;) 
trying to actually make a decent tool that can help me create better tools for other people

Someday i have to properly write this

## Features

### Skipping fields entirely
### Change the display name of fields
### Exposing non-serialized/runtime fields
### Use the default editors on demand
### React to input changes
### Hide serialized fiends from the editor
### Easily create buttons that execute methods
### Create readonly fields on the editor (that can be edited in code)

## Usage

Anotate the parent class that you want a custom editor for with the ``UnnamedUiToolkitAttribute``

Just anotate the classes with attributes to change the editor behavior

You can find them under ``Runtime/EditorDrawer/CustomAttributes``


## Status

On development, accepting feature requests


## Note to self


### Work in progress

#### Implement update callback

Easy as cacke, programatically trigger a rebind event that persist data updated by events like:

* Button
* OnChange

#### Cache all reflection

This will improve performance on large editors

Currently all the interactions with the classes are already creating cached compiled delegates

ToDo => 
    Cache the ``TreeHandler`` per type so it doesn't get recalculated each time you switch on an editor or 
    if it appears several times on the hierarchy 



## Other notes

This attributte exists and it's super useful, use it

[DebuggerDisplay(value: "{AngleDelta}", Name = "{Point}")]