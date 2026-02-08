# XOMI                                                                                          
XOmi is a small tool that listen to UDP message to execute Xbox commands.  
  
> Note :Code is not clean or beautiful at all.  
> But is works and that good enough for me for now.  


You can test your code with this tool:
https://github.com/EloiStree/2025_05_29_UnityProject_XInputFourDroneDebug



Need ViGEmBus: https://github.com/ViGEm/ViGEmBus/releases/tag/v1.21.442.0   
C# 9.0: To Add  


# V0.1: Integer version

See the mapping of the key here:  
[https://github.com/EloiStree/2024_08_29_ScratchToWarcraft](https://github.com/EloiStree/2024_08_29_ScratchToWarcraft)  
  

How to use:   
[![image](https://github.com/user-attachments/assets/6c8f6fb5-9744-437a-ae04-8941b225b7a5)](https://youtu.be/xVfCwnrzutk)  
[Draft and long step by step](https://youtu.be/xVfCwnrzutk)   
  


# Xbox Xinput version


**XInput is finite and won't change over time.** Therefore, the following code should work for most of my future tools.  
**Why include XInput code in a Scratch to Warcraft project?**  
Because:  
- This project serves as the landing page for this workshop. 😋  
- I plan to support a Warcraft version with split-screen gameplay using four XInput gamepads.

**Preferred versions for your code:**
- **`1899887766`**: To manage the left and right joysticks of the gamepad using 4 bytes.  
- **`1700000000`**: To manage all the buttons using 4 bytes.  

**Bluetooth ESP32 Version:** [GitHub Repository](https://github.com/EloiStree/2024_11_21_ESP32HC05RC/blob/main/XESP32HC05RC/XESP32HC05RC.ino)_  



If you can send Index Integer, use index 0 for all and 1-4 to choose the controller.
IF you can only use Integer, use 1899 format. See below



| **Short Description**                     | **Integer to Press** | **Integer to Release** |
|-------------------------------------------|-----------------------|-------------------------|
| Random input for all gamepads, no menu    | 1399                 | 2399                   |
| Enable hardware joystick ON/OFF           | 1390                 | 2390                   |
| Press A button                            | 1300                 | 2300                   |
| Press X button                            | 1301                 | 2301                   |
| Press B button                            | 1302                 | 2302                   |
| Press Y button                            | 1303                 | 2303                   |
| Press left side button                    | 1304                 | 2304                   |
| Press right side button                   | 1305                 | 2305                   |
| Press left stick                          | 1306                 | 2306                   |
| Press right stick                         | 1307                 | 2307                   |
| Press menu right                          | 1308                 | 2308                   |
| Press menu left                           | 1309                 | 2309                   |
| Release D-pad                             | 1310                 | 2310                   |
| Press arrow north                         | 1311                 | 2311                   |
| Press arrow northeast                     | 1312                 | 2312                   |
| Press arrow east                          | 1313                 | 2313                   |
| Press arrow southeast                     | 1314                 | 2314                   |
| Press arrow south                         | 1315                 | 2315                   |
| Press arrow southwest                     | 1316                 | 2316                   |
| Press arrow west                          | 1317                 | 2317                   |
| Press arrow northwest                     | 1318                 | 2318                   |
| Press Xbox home button                    | 1319                 | 2319                   |
| Random axis                               | 1320                 | 2320                   |
| Start recording                           | 1321                 | 2321                   |
| Set left stick to neutral (clockwise)     | 1330                 | 2330                   |
| Move left stick up                        | 1331                 | 2331                   |
| Move left stick up-right                  | 1332                 | 2332                   |
| Move left stick right                     | 1333                 | 2333                   |
| Move left stick down-right                | 1334                 | 2334                   |
| Move left stick down                      | 1335                 | 2335                   |
| Move left stick down-left                 | 1336                 | 2336                   |
| Move left stick left                      | 1337                 | 2337                   |
| Move left stick up-left                   | 1338                 | 2338                   |
| Set right stick to neutral (clockwise)    | 1340                 | 2340                   |
| Move right stick up                       | 1341                 | 2341                   |
| Move right stick up-right                 | 1342                 | 2342                   |
| Move right stick right                    | 1343                 | 2343                   |
| Move right stick down-right               | 1344                 | 2344                   |
| Move right stick down                     | 1345                 | 2345                   |
| Move right stick down-left                | 1346                 | 2346                   |
| Move right stick left                     | 1347                 | 2347                   |
| Move right stick up-left                  | 1348                 | 2348                   |
| Set left stick horizontal to 1.0          | 1350                 | 2350                   |
| Set left stick horizontal to -1.0         | 1351                 | 2351                   |
| Set left stick vertical to 1.0            | 1352                 | 2352                   |
| Set left stick vertical to -1.0           | 1353                 | 2353                   |
| Set right stick horizontal to 1.0         | 1354                 | 2354                   |
| Set right stick horizontal to -1.0        | 1355                 | 2355                   |
| Set right stick vertical to 1.0           | 1356                 | 2356                   |
| Set right stick vertical to -1.0          | 1357                 | 2357                   |
| Set left trigger to 100%                  | 1358                 | 2358                   |
| Set right trigger to 100%                 | 1359                 | 2359                   |
| Set left stick horizontal to 0.75         | 1360                 | 2360                   |
| Set left stick horizontal to -0.75        | 1361                 | 2361                   |
| Set left stick vertical to 0.75           | 1362                 | 2362                   |
| Set left stick vertical to -0.75          | 1363                 | 2363                   |
| Set right stick horizontal to 0.75        | 1364                 | 2364                   |
| Set right stick horizontal to -0.75       | 1365                 | 2365                   |
| Set right stick vertical to 0.75          | 1366                 | 2366                   |
| Set right stick vertical to -0.75         | 1367                 | 2367                   |
| Set left trigger to 75%                   | 1368                 | 2368                   |
| Set right trigger to 75%                  | 1369                 | 2369                   |
| Set left stick horizontal to 0.5          | 1370                 | 2370                   |
| Set left stick horizontal to -0.5         | 1371                 | 2371                   |
| Set left stick vertical to 0.5            | 1372                 | 2372                   |
| Set left stick vertical to -0.5           | 1373                 | 2373                   |
| Set right stick horizontal to 0.5         | 1374                 | 2374                   |
| Set right stick horizontal to -0.5        | 1375                 | 2375                   |
| Set right stick vertical to 0.5           | 1376                 | 2376                   |
| Set right stick vertical to -0.5          | 1377                 | 2377                   |
| Set left trigger to 50%                   | 1378                 | 2378                   |
| Set right trigger to 50%                  | 1379                 | 2379                   |
| Set left stick horizontal to 0.25         | 1380                 | 2380                   |
| Set left stick horizontal to -0.25        | 1381                 | 2381                   |
| Set left stick vertical to 0.25           | 1382                 | 2382                   |
| Set left stick vertical to -0.25          | 1383                 | 2383                   |
| Set right stick horizontal to 0.25        | 1384                 | 2384                   |
| Set right stick horizontal to -0.25       | 1385                 | 2385                   |
| Set right stick vertical to 0.25          | 1386                 | 2386                   |
| Set right stick vertical to -0.25         | 1387                 | 2387                   |
| Set left trigger to 25%                   | 1388                 | 2388                   |
| Set right trigger to 25%                  | 1389                 | 2389                   |
| Release ALL Touch                  | 1390                 | 2390                   |
| Release ALL Touch  but menu        | 1391                 | 2391                   |
| Clear Timed Command        | 1398                 | 2398                   |

When you create a tunnel on a shared server, you can only send integers in my toolboxes.
If you are in this case, I created a format called 1899 for that.

`8899666666`: 88 Player, 99 Value Type, 666666 Value

| Code       | Description                                      |
|------------|--------------------------------------------------|
| 01 19 0000000 | Scratch To Warcraft Tables 1000 2000             |
| 01 20 9 9 9 9 9 9 | Left X Y Right X Y Trigger L R                   |
|            | Axis 0 = Zero, 1 = -1 , 9 = 1                   |
|            | Joysticks 0 = Zero, 1 = 0 , 9 = 1               |
| 01 21 999 999 | Left XY  0 = Zero, 001 = -1 , 999 = 1           |
| 01 22 999 999 | Right XY  0 = Zero, 001 = -1 , 999 = 1           |
| 01 23 999999 | Percent -1(000000) to 1(999999) Left X         |
| 01 24 999999 | Percent -1(000000) to 1(999999) Left Y         |
| 01 25 999999 | Percent -1(000000) to 1(999999) Right X        |
| 01 26 999999 | Percent -1(000000) to 1(999999) Right Y        |
| 01 27 999999 | Percent 0(000000) to 1(999999) Trigger Left    |
| 01 28 999999 | Percent 0(000000) to 1(999999) Trigger Right    |

More about it: [GitHub - 1899 Format](https://github.com/EloiStree/OpenUPM_PushGenericIID/tree/main/Runtime/Unstore/1899)
