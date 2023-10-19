# Chinook

This application is unfinished. Please complete below tasks. Spend max 2 hours.
We would like to have a short written explanation of the changes you made.

1. Move data retrieval methods to separate class / classes (use dependency injection)
2. Favorite / unfavorite tracks. An automatic playlist should be created named "My favorite tracks"
3. The user's playlists should be listed in the left navbar. If a playlist is added (or modified), this should reflect in the left navbar (NavMenu.razor). Preferrably, the left menu should be refreshed without a full page reload.
4. Add tracks to a playlist (existing or new one). The dialog is already created but not yet finished.
5. Search for artist name

When creating a user account, you will see this:
"This app does not currently have a real email sender registered, see these docs for how to configure a real email sender. Normally this would be emailed: Click here to confirm your account."
After you click 'Click here to confirm your account' you should be able to login.

Please put the code in Github. Please put the original code (our code) in the master branch, put your code in a separate branch, and make a pull request to the master branch.


Dev Note:-
1. Implemented all the above mentioned features
2. Implemented search artist feature on search box text changes
3. Followed the proper C# naming conventions and changed some varibale names.
4. Used "CascadingValue" to share the status among multiple components. Used to keep and use the Playlists to show in NavMenu (with real time update) and in Add tracks to playlist dialog
5. Used constant for "My favourite" playlist
6. Check for duplicate playlists while creating a playlist and check for duplicate tracks in the same playlist while adding the tracks to the playlist (with exception handling)
7. Configured the fluent API to generate automatic Id for playlists
8. Use automapper to map client models and data models.