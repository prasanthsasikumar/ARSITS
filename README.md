# ARSITS [Augmented Reality Spatial Instruction Training System]

[![N|Solid](https://github.com/prasanthsasikumar/localMultiplayer/blob/master/powerdByLogo.png)](http://empathiccomputing.org/)

[![Build Status](https://travis-ci.org/joemccann/dillinger.svg?branch=master)](https://github.com/prasanthsasikumar/ARSITS)

ARSITS: Augmented Reality based Spatial Instruction Training System

Project description would be updated here by the end of this month(march 2020)


This section explains the implementation of VR set up used. We used two VR headsets( HTC vive) for the experiment. Both the devices were synchronized over a network connection.


# HARDWARE REQUIREMENTS
- HTC VIVE (prefer pro - as some see through samples are also included)
- Video feed saved from Kinect or kinect sensor itself (We have used Three kinnect sensors)
- Meta 2 AR Display
- We have used Unity 2019.3.2f1, and is not really backwards compatible due to unity HDRP. 

# CREDITS
Keijiro https://github.com/keijiro/Akvfx


# TECHNICAL SPECIFICATIONS


### Structure
- Main scene name - Combined

###### Explanation of Components: 
- Game Design Logic - Drum game stuff including all the spawn points, the rings that fall down etc are managed by this.
- NetworkManager - Manages all the application parameters remotely. Application Manager script has the toggles for all the parameters
- InteractionModule - Takes care of the controllers. Map the controllers to the sticks
- Camera Module - Functions relating to camera(intel D415), and the graphics processing of the image input is done here
- Camera Rig - Under camera, the camera module attaches itself on runtime to get the particle effect.

### Interaction
- Attach controllers at the end of the drumsticks for tracking
- Produces circles to hit based on music
- changes visualizations based on PLV(brain Synchronization).


### Downloads(Source code)
- Please find the source code here - https://github.com/prasanthsasikumar/ARSITS
- Issues can be reported here - https://github.com/prasanthsasikumar/ARSITS/issues/new



### Todos

 - Add the trigger application(MAX)
 - A lot more work
 
 ### Videos
 - 

License
----

MIT


**Free Software, Hell Yeah!**
