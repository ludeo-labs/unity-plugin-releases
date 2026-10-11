# Release notes - Ludeo Company - Unity SDK 
# v4.3.4
- **Description:**
	- Switched to the CoreSDK 4.2.9
		- Moments are now recorded in AV1 where the hardware supports it (NVENC AV1, then NVENC H.264, then libx264). GPUs without AV1 support get H.264 as before
		- Encoding can be controlled per game from Studio Lab under `videoEncoderConfig` (`hardwareEncoder`, `av1Config`)
		- A stopped encoder is replaced by the next one in the list instead of ending video for the session, and clips start closer to the marked moment
		- Fixed a GPU hang (`DXGI_ERROR_DEVICE_HUNG`) when recording a D3D11 game whose device isn't multithread-protected, and several NVENC/AMF stall and shutdown issues
		- The bundled Win64 DLLs are Authenticode-signed (the 4.3.3 package still carried the 4.2.7 binaries)
	- Fixed crossed callbacks when `CloseRoom`, `AddPlayer` or `RemovePlayer` calls overlap. Each call now gets its own callback; previously a second call overwrote the first one's callback, and two overlapping `CloseRoom` calls threw a `NullReferenceException`
- **Impact:**
	- `CloseRoom` disposes the room it was called on (not `LudeoRoom.ActiveRoom`), and now disposes it even when the native call throws
	- If the native `AddPlayer` call throws, the callback now runs with `LudeoResult.WrapperException` instead of never being called
-**Action Required:**
# v4.3.3
- **Description:**
	- Switched to the CoreSDK 4.2.8
		- The Windows Core SDK DLL is now Authenticode-signed and carries company, product, description and copyright fields in its file properties
	- Renamed the Steam-related `LudeoSettings` inspector labels to "Run Without Steam Client", "Steam User Id" and "Beta Version Name", and reworded their tooltips
- **Impact:**
	- Inspector labels only; the serialized field names are unchanged
-**Action Required:**
# v4.3.2
- **Description:**
	- Switched to the CoreSDK 4.2.7
- **Impact:**
-**Action Required:**
# v4.3.1
- **Description:**
	- Switched to the CoreSDK 4.2.5
- **Impact:**
-**Action Required:**
# v4.2.0
- **Description:**
	- Switched to the CoreSDK 4.2.0
	- Aligned the public API with the Unreal plugin:
		- `LudeoGameplaySession` → `LudeoPlayer` (`Begin`/`End`/`Abort` → `BeginGameplay`/`EndGameplay`/`AbortGameplay`)
		- Object writing/actions moved to `LudeoRoom.Writer` (`LudeoRoomWriter`); session creation moved to `LudeoManager.SessionManager`
		- Notifications are now C# events on `LudeoSession` (replacing `AddNotify*`/`RemoveNotify*`)
		- Added `Task`-returning `*Async` overloads for all async operations
	- New: thread-safe DataWriter via `LudeoSessionOpenRoomParameters.ThreadSafe` / `MaxConcurrentWrites`
	- New: `LudeoRoomWriter.SetSendSettings(uint sendIntervalMs)`
- **Impact:**
	- Breaking API changes (no compatibility shims). `LudeoDataReader` was intentionally not renamed.
-**Action Required:**
	- Migrate existing integrations to the new API — see `UPGRADING.md`.
# v4.0.22
- **Description:**
	- Switched to the CoreSDK 4.0.21 
- **Impact:**
-**Action Required:**
# v4.0.21.1
- **Description:**
	- Switched to the CoreSDK 4.0.13.1
- **Impact:**
	Hotfix for PS5 controller for Lost Castle
# v4.0.21
- **Description:**
	- Switched to the CoreSDK 4.0.13 
- **Impact:**
-**Action Required:**
# v4.0.20
- **Description:**
    - Modified RunCommand so it uses the new video encoder system
    - Fixed issue with ludeo auto start doesn't kick off 
    - 
- **Impact:**
  - The ludeo auto start works as intended
  - The video upload doesn't get stuck anymore
-**Action Required:**


## 🆕 New Features
[LC-8973] Lost Castle - Highlights are stuck at "waiting" status in drafts
[LC-8977] Lost Castle - Latest Unity SDK & Sample game do not work as expected and/or documented


# v4.0.19
## ⚠️ Breaking Changes[CodeInitialization.md](..%2FLudeoSDKDocumentation%2Fpages%2FCodeInitialization.md)
- **Description:**
    - [LC-8531] Seperate the init flow for the LudeoSDK.
    - this allows for a more controlled init flow for the LudeoSDK.
    - No obligatory callbacks (though for all the sdk features to function all the callbacks should be implemented).
- **Impact:**
  - `InitLudeoSession` is now async. The function expects a callback function that will be invoked with the newly created `LudeoSession`. 
  - Activate the `LudeoSession` from the callback by calling `Activate`.
-**Action Required:**
  - Add callback to the `InitLudeoSession` function.
  - Register to the required notifications of the created `LudeoSession`.
  - After registering to all the notifications, Activate the `LudeoSession`.


## 🆕 New Features
[LC-7873] Add localization api the different Ludeo SDK views
[LC-8531] Update Core SDK version to 4.0.12

# v4.0.18

## ⚠️ Breaking Changes[CodeInitialization.md](..%2FLudeoSDKDocumentation%2Fpages%2FCodeInitialization.md)

- **Description:**
    - [LC-7353] Allow the game to mute when Playable moments is open 
- **Impact:**
    - Calls to the `InitLudeoSession(LudeoSessionCallbacks ludeoSessionCallbacks)` method will fail.
    - A new callback was introduced to handle the mute/unmute events `LudeoSessionCallbacks.muteRequestCallback` of type `Action<LudeoSessionMuteRequestCallbackData>`.
- **Action Required:**
    - Add an implementation for the new callback `LudeoSessionCallbacks.muteRequestCallback` to handle the mute/unmute requests from Ludeo.

## 🆕 New Features

[LC-7349] Support Domain Reload disable

