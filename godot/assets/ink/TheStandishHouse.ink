The Standish House, a story of discovery

-> pub

=== pub ===
Your travels have brought you to an inn in the town of Beeston, most known for the old 13th century castle. The inn is bustling for a wednesday afternoon and e few villager are playing a card game that has them entertained.
    -> table
= table
You sit down at an empty table.
* [Talk to the innkeeper] 
    You walk up to the bar for a chat with the innkeeper 
    ->innkeeper
* [Observe the card game] -> cards
* {directions} [Head for the house] -> outside

= innkeeper
The innkeeper looks at you like the foreigner that you are and nods in the direction of an open table indicating that you should sit. 
    + [Return to your table] -> table
    * [Ask about sights] -> sights
    * {sights} [Get Directions] -> directions
    
= sights
    You ask the innkeeper if there is anything worth visiting around here. He begins to tell you about the castle but sees your attention wander, he says "If you're looking for something more off the beaten path you should go explore the Standish house, our local haunted house" 
    -> innkeeper

= directions
    It's just a short walk to the house the innkeeper described. 
    -> innkeeper

= cards
The card game is in full swing, with everybody engaged in every hand. Some money is being exchanged but you really can't figure out what game is being played. One of the players is eying you and you have the impression its better to leave the players at their game.
-> table

=== outside ===
You leave the patrons of the inn to their game and head out to the street you
* [To the House] -> house_front_gate

=== house_front_gate ===
After a short walk you walk up to a large somewhat disheveled house. A rusty iron gate blocks your path -> END
