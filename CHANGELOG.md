# Changelog

Changelog for the HRA Organ Gallery

## 0.6.3 - 2022-03-21

### Added in 0.6.3

* Adjusted display of CTs per AS in selected tissue block
* Solved bug where multiple tissue blocks were highlighted when hovering over neighboring tissue blocks
* Added new splash screen

## 0.6.2 - 2022-03-17

### Added in 0.6.2

* Added code to get all cell types to be expected in selected tissue block (on click)
* cleaned code in Main scene

## 0.6.1 - 2022-03-04

### Added in 0.6.1

* Removed code at scene load that checks for cell type counts on GitHub (speeds up scene load significantly)
* Grouped dragon model in skin F left hand to Environment game object

## 0.6 - 2022-01-13

### Added in 0.6

* Complete redesign of stages (now all in one scene)
* App now pulls organs and tissue blocks dynamically from [CCF API](https://ccf-api.hubmapconsortium.org)
* Added basic extrusion system to lay out organs in 2D by body system and individually with input from controller
* Basic dev UI display showing number of tissue blocks, matching cell type counts founds on GitHub, and tissue blocks with such cell type counts

#### Known issues
* You need an internet connection to use the application. 
* Framerate drops occur when more than 3-4 organs are in the viewport at once
* Extreme framerate drops occur while organs are loaded (~5-10 seconds after application starts)
* Rendering issues cause lack of transparency between organs and tissue blocks 
* Jagged edges along text labels and background
* Wrongly registered tissue blocks appear to float in space when extruded
* Light sources sometimes flicker in and out
* When collapsing organs back into the body, there is a joystick value mismatch, causing the organs to partially come out again
* The mammary glands are placed close to our outside of the boundaries of the teleportable floor when fully extruded 

## 0.5 - 2022-01-05

### Added in 0.5

* Added new CCF reference organs from 12/2022 release
* Brought the female in Stage 1 closer to the user
* Added labels over each organ on Stage 2
* Made the required play area smaller so the user can reach the female in Stage 1 and the kidney box in Stage 3 more easily
* Add teleportation to Stages 1 and 3 

## 0.4.1 - 2021-12-29

### Added in 0.4.1

* Adjusted height of skin reference organ in Stage 1
* Made outline of anatomical structures thinner in Stage 2
* Added annotation that #cells in the entire organ is actually larger in Stage 3
* Replaced the music

## 0.4.0 - 2021-11-27

### Added in 0.4.0

* Added Stage 3 (cells) with CellxGene data for the male left kidney
* Refined Stage 1 with light effects
* Implemented basic SceneSelector via spheres
* Added annotations for all stages
* Added music (for testing purposes) 

## 0.3.5.1 - 2021-10-26

### Added in 0.3.5.1

* Replaced highlight marker with outline
* Implemented simple annotation for controller

## 0.3.5 - 2021-10-19

### Added in 0.3.5

* More organs added
* Fixed issue where organ explosion value was resized during user input even when not selected
* Added organ selector and highlighter
* Added teleportation platforms

## 0.3.4.1 - 2021-10-6

### Added in 0.3.4.1

* Initial commit to GitHub
* First build in alpha channel shared with other users via App Lab
* Individual organ view with heart and lung