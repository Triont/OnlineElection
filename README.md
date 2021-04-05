# OnlineElection
This is ASP.NET Core MVC application which can be used for create online elections and do votes there by authenticated users who have a confirmed email. 
Elections' results has possibility to be seen by all users. 
##
In application was implement hostservice for monitoring correct duration of elections.
Users can reset forgotten password and edit account info.
###
For confirm email or reset forgot password here will be generated an unique token based on user info and GUID value without Identity. Generated token 
will be automaticaly sent to user's email address.
Users can found necessary election using found by election name or election id, or candidates' names. Elections, which duration expired,
will be marked as "Archived", here will be impossible to vote, but result can be viewed.