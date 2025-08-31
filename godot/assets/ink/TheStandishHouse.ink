-> pub
// External functions
// Run an encounter with the character of the given id
EXTERNAL Encounter(id)
// Give the player character an item
EXTERNAL Gain(id)



// Placeholder functions for EXTERNAL 
=== function Encounter(x) ===
~ return "The player encountered " + x

=== function Gain(x) ===
~ return "The player gained " + x

=== pub ===
Some travels have brought you to an inn in the town of Beeston, most known for the old 13th century castle. The inn is bustling for a wednesday afternoon and e few villager are playing a card game that has them entertained.
    -> table
= table
You sit {table > 1: back } down at an empty table. 
{cards:  While you were watching the card game the innkeeper put down some cutlery on you table.}
-> at_table

= at_table

    * [Test Encounter] -> encounter_innkeeper
    + [Talk to the innkeeper] 
        You walk up to the bar for a chat with the innkeeper 
        ->innkeeper
    * [Observe the card game] -> cards
    * (examined) {cards} Examine the cuttlery 
        It's a fairly standard set, fork, knife, spoon. You notice though that the knife has a nasty tip on it, you would want to avoid getting stuck with that.
        -> at_table 
    * {examined} Take knife
        You try and sneakily put the knife in your pocket, but the innkeeper spots you and yells "Yo, that's not yours". He walks up to you and starts a tussle. 
        -> encounter_innkeeper
// Add external function for a fight, push the name of the 
// thing that is being fought and a variable for success/failure 
// the variable gets set to t he correct value if the encounter 
// succeeded or failed.
        -> at_table
    * {directions} [Head for the house] -> outside

= encounter_innkeeper
    You and the innkeeper are fighting over control of the knife.
    ~ Encounter("beeston_innkeeper")
    + [Defeat] 
        The innkeeper wrests control of the knife from you and puts it back onto the table. -> at_table
    + [Victory] 
        You take the knife and put it in your bag. 
        ~Gain("beeston_knife")
        -> at_table

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

After a short walk you walk up to a large somewhat disheveled house. A rusty iron gate blocks your path
    * (open_gate) [Open Gate] 
        You try to open the gate but while it rattles a bit it won't budge. You notice an old fashion bell that is attached to the side of the gate.
        -> house_front_gate
    * {open_gate} [Push Harder]
        You decide to push a little bit harder and after a small struggle whatever was jammed gives in and you can open the gate. As soon as you cross the line of the garden fence you hear something stir in the back of the garden and you see a large crow rising up. The crow squawks a couple of times and then flies directly towards you
            -> encounter_crow
    * [Walk around the house]
        You spend some time looking around the house but there is a thick thorny hedge surrounding it and you really don't want to break any of the widnows.
        -> house_front_gate
= encounter_crow
    ~ Encounter("large_crow")
    -> DONE
    