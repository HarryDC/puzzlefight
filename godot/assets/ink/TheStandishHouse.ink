-> pub
// External functions
// Run an encounter with the character of the given id
EXTERNAL Encounter(id)



// Placeholder functions for EXTERNAL 
=== function Encounter(x) ===
~ return 1


=== pub ===
Your travels have brought you to an inn in the town of Beeston, most known for the old 13th century castle. The inn is bustling for a wednesday afternoon and e few villager are playing a card game that has them entertained.
    -> table
= table
You sit {table > 1: back } down at an empty table. 
{cards:  While you were watching the card game the innkeeper put down some cutlery on you table.}
-> at_table

= at_table

    * [Test Encounter] -> encounter_innkeeper
    * [Talk to the innkeeper] 
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
    ~ Encounter("innkeeper")
    * [Defeat] -> at_table
    * [Victory] -> at_table

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
    * {open_gate} [Ring Bell]
        The bell makes a very broken sound and at first nothing happens but after a pause of a few seconds suddenly the bell grows to human size and plants itself right in front of you. It's ready for a tussle. 
            -> END
    * [Walk around the house]
        You spend some time looking around the house but there is a thick thorny hedge surrounding it and you really don't want to break any of the widnows.
        -> house_front_gate
        