using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NLGManager : MonoBehaviour {

    //I place both verbs and nouns in arrays so that I can access each object smoothly by way of tracking indices.
    //create nounarray outside of start(?)

#pragma warning disable 0219 //NEVER FORGET ABOUT THIS

    Noun killer = new Noun("killer", false), enemy = new Noun(false, "enemy", "enemies", true), //I can't have 'enemys.'
    exile = new Noun("exile", true), imp = new Noun("imp", true), knight = new Noun("knight", false), sword = new Noun("sword", false),
    axe = new Noun("axe", true), gap = new Noun("gap", false), drop = new Noun("drop", false), wall = new Noun("wall", false),
    platform = new Noun("platform", false), Disciple = new Noun(true, "Disciple", false), Nittonio = new Noun(true, "Nittonio", false),
    Sevryna = new Noun(true, "Sevryna", false), Panic = new Noun(true, "Panic", false), Qitma = new Noun(true, "Qitma", false),
    Qalem = new Noun(true, "Qalem", false), Kenzin = new Noun(true, "Kenzin", false), Infinite_Domain = new Noun(true, "Infinite Domain", true),
    Tear_in_the_Void = new Noun(true, "Tear in the Void", "Tears in the Void", false), Twilight_Valley = new Noun(true, "Twilight Valley", false),
    Rotwood_Keep = new Noun(true, "Rotwood Keep", false), Shrieking_Woods = new Noun(true, "Shrieking Wood", "Shrieking Woods", false),
    Halls_of_Panic = new Noun(true, "Hall of Panic", "Halls of Panic", false), No_Mans_Pass = new Noun(true, "No Man's Pass", "No Man's Passes", false),
    Tomb_of_the_Destroyer = new Noun(true, "Tomb of the Destroyer", "Tombs of the Destroyer", false), Kings_Path = new Noun(true, "King's Path", false),
    Faultless_Palace = new Noun(true, "Faultless Palace", false);

    Verb kill = new Verb("kill"), die = new Verb("to die", "die", "dies", "dying", "died", "died"),
    fall = new Verb("fall", "fell"), jump = new Verb("jump"), leap = new Verb("leap"),
    fly = new Verb("to fly", "fly", "flies", "flying", "flew", "flown"),
    be = new Verb("to be", "am", "is", "are", "was", "been"), //Due to the verb's irregularity, I should consider the use of errant strings instead of Verb objects.
    respawn = new Verb("respawn"), stab = new Verb("stab", "stabbing", "stabbed"),
    run = new Verb("run", "running", "ran", "run"),
    obliterate = new Verb("obliterate", "obliterating", "obliterated"),
    smash = new Verb("to smash", "smash", "smashes", "smashing", "smashed", "smashed"),
    ricochet = new Verb("ricochet"), recoil = new Verb("recoil"),
    push = new Verb("to push", "push", "pushes", "pushing", "pushed", "pushed"),
    pull = new Verb("pull"), force = new Verb("force", "forcing", "forced"),
    stop = new Verb("stop", "stopping", "stopped"), avoid = new Verb("avoid"),
    bolt = new Verb("bolt"), move = new Verb("move", "moving", "moved");

    public bool hasFallen = false, hasBeenStabbed = false, hasStabbed = false, quickSuccessionFalls = false,
        quickSuccessionStabs = false, hasKilledBoss = false, boltMeterEmpty = false, boltUnusedForLong = false, 
        aerialKill = false;

    public int deathsDuringLevel = 0, deathsDuringGame = 0,  killsDuringLevel = 0, killsDuringGame = 0, inAir = 0;

    private int last = 0;

    public float distFromGoal = 0;

    public string[] mildlySadWords = { "out-of-sorts", "unfortunate", "regrettable", "pitiful", "pitiable" };

    public string[] sadWords = { "blah", "lamentable", "disappointing", "lame", "dismal" };

    public string[] verySadWords = { "disgusting", "reprehensible", "disgraceful", "miserable", "traumatic" };

    public string[] theAdverbs = { "truly", "ostensibly", "harshly", "grotesquely" };

    public string[] subjectPronouns = {  "he", "she", "who", "whoever", "you", "I", "we", "they" };

    public string[] objectPronouns = {  "him", "her", "whom", "whomever", "you", "me", "us", "them" };

    public string[] prepositions = { "with", "in", "at", "by", "before", "between", "from", "on", "over", "to" };

    public string[] conjunctions = { "for", "nor", "or", "and", "but", "yet", "so" };

    public string[] articles = { "an", "a", "the", "that", "this", "every", "each", "some", "most", "these", "those"};

    public string[] whWords = { "who", "what", "when", "where", "why", "how" };

    public string S = "";

    void Update () {
        if ((aerialKill || hasFallen || hasStabbed || hasBeenStabbed || boltMeterEmpty || quickSuccessionFalls || quickSuccessionStabs || hasKilledBoss))
        {
            ruleSwitch();
        }
	}

    void ruleSwitch()
    {
        Noun[] nounArray = { killer, enemy, exile, imp, knight, sword, axe, drop, gap, wall, platform, Disciple,
        Nittonio, Sevryna, Panic, Qitma, Qalem, Kenzin, Twilight_Valley, Rotwood_Keep, No_Mans_Pass, Infinite_Domain, Tear_in_the_Void, Shrieking_Woods, Halls_of_Panic, Tomb_of_the_Destroyer,
        Kings_Path, Faultless_Palace };

        //12 through 20 can't use "the"
        Verb[] verbArray = { kill, die, fall, jump, leap, fly, respawn, run, stab, obliterate, smash, ricochet, recoil, push, pull, force, stop, be, avoid, bolt, move };

        //Grammar Rules... Still working on precedence and creating syntactical conditionals, most of which will be random...
        string NP = "", NPObj = "", VP = "", AdjectiveVar = sadWords[Random.Range(0,4)], AdverbVar = theAdverbs[Random.Range(0,3)], ConjunctionVar = conjunctions[Random.Range(0,6)], PrepositionVar = prepositions[Random.Range(0,9)], ArticleVar = articles[Random.Range(0,8)], sPronounVar = subjectPronouns[Random.Range(0,7)], oPronounVar = objectPronouns[Random.Range(0,7)], whWord = whWords[Random.Range(0,5)];
        int temp = Random.Range(0, 27); //Choose among the nouns (for the subject).
        int tempObj = Random.Range(0, 27); //Choose among the nouns (for the object).
        int select = Random.Range(0, 11);
        int singularOrPlural = Random.Range(0, 1); //Choose whether the subject will be singular or plural.
        int singularOrPluralObj = Random.Range(0, 1); //Choose whether the object will be singular or plural.
        int tenseSwitch = Random.Range(0, 4); //Choose among the existing tenses.
        int useGeneration = Random.Range(0, 2);
        if (select == last)
        {
            ruleSwitch();
        }
        if (useGeneration == 0 || boltMeterEmpty)
        {
            if (quickSuccessionFalls)
            {
                if (boltUnusedForLong)
                {
                    string[] sentences = { "death comes for all who fail to use bolt.", "you have abilities.  you should consider using them.", "you don't seem to be understanding this whole bolt thing.", "the bolt ability seems not to be getting through this one's mind", "avert death with dexterity.", "dexterity might make your lives a little easier.", "you could try bolting.  you can't possibly die more often.", "bolting, shmolting.  though it could seriously help you, genius.", "you could avoid death with some speed.", "use bolt, every once in a while.", "a running start wouldn't kill you.", "move. fast. then. jump. good lord." };
                    Debug.LogWarning("NON-GENERATED: " + sentences[select]);
                    S = sentences[select];
                    last = select;
                    quickSuccessionFalls = false;
                    hasFallen = false;
                    boltUnusedForLong = false;
                }
                else if (inAir > 55)
                {
                    string[] sentences = { "'Look at me.  I'm the Disciple, and I'm the best person here at killing myself.'", "Not only are you getting sweet air, but you're getting the sweet release of death, too.", "It's not the fall that bores you.  It's the impact.", "Don't you become tired of that wretched noise?", "Get a life.", "Masochism becomes monotony.", "You're not going to move the planet.", "You can try to move the planet.  It's legal.", "You are as graceful as a meteor piloting a collapsing building.", "I feel worse for the floor than I do for you.", "You've shown the ground who's boss.", "Praise the ground." };
                    //Debug.LogWarning("NON-GENERATED: " + sentences[select]);
                    S = sentences[select];
                    last = select;
                    quickSuccessionFalls = false;
                    hasFallen = false;
                }
                else if (distFromGoal > 125)
                {
                    string[] sentences = { "somebody's frustrated.", "i fear you fail to see the point.", "This is not much of a plan.", "Consider jumping.  Try it.", "You know, these surfaces aren't that slippery.", "Have you any hobbies?", "Do you know what a hobby is?", "You are truly amoebic.", "You must be AFK.", "That's beyond bad.", "You're extremely coordinated.", "Crazy or stupid?" };
                    //Debug.LogWarning("NON-GENERATED: " + sentences[select]);
                    S = sentences[select];
                    last = select;
                    quickSuccessionFalls = false;
                    hasFallen = false;
                }
                else if (distFromGoal > 60 && distFromGoal < 125)
                {
                    string[] sentences = { "i don't think you're lazy.  i know you're nuts.", "certainly, you have a motive.", "I'm sure you have an excellent reason for this.", "You have a good reason to do this. I'm sure.", "Perhaps you are not as amoebic as I previously thought.", "You're putting in a strange amount of effort in the realm of failing.", "I don't understand.", "Just enough effort to kill yourself.  Fascinating.", "How incompetent can you possibly be?", "Are you, like, two?", "Unbelievable, you clown.", "Oh, wow." };
                    //Debug.LogWarning("NON-GENERATED: " + sentences[select]);
                    S = sentences[select];
                    last = select;
                    quickSuccessionFalls = false;
                    hasFallen = false;
                }
                else
                {
                    string[] sentences = { "you are adept at falling.", "falling into oblivion constantly is no way to go through life.", "Enough with the falling.", "Do something beside falling.  Just a suggestion.", "Keep typing on the keyboard, and you'll drive yourself crazy.", "You'd be making a lot more headway by doing nothing.", "Doing nothing would get you just as far.", "Stop touching stuff.", "Stop touching the keyboard for a moment.", "Calm down.", "There's more to this game than falling down and dying.", "Avoiding death might be worth your while." };
                    //Debug.LogWarning("NON-GENERATED: " + sentences[select]);
                    S = sentences[select];
                    last = select;
                    quickSuccessionFalls = false;
                    hasFallen = false;
                }
            }

            else if (hasFallen)
            {
                //Debug.LogWarning(distFromGoal);
                if (boltUnusedForLong)
                {
                    string[] sentences = { "learn how to bolt.", "it's time to use bolt.", "Think about using bolt at least once.", "Bolt might help you once or twice.", "Bolting is helpful.  I swear.", "I promise bolting will help you in the long run.  No pun intended.", "Hit 'shift' every now and then.", "Come on.  Use shift.", "Avoid death with some speed.", "Bolt, every once in a while.", "A running start wouldn't kill you.", "Move. Fast. Then. Jump. Good lord." };
                    //Debug.LogWarning("NON-GENERATED: " + sentences[select]);
                    S = sentences[select];
                    last = select;
                    hasFallen = false;
                    boltUnusedForLong = false;
                }
                else if (deathsDuringLevel % 60 == 0 && deathsDuringLevel >= 60)
                {
                    string[] sentences = { "you're becoming quite the base jumper.", "death comes for us all.  death comes for us a lot.", "Falling is overrated.", "If only I had a nickel for every time you did this.", "You're loving this.", "No one should want to do this.", "Maybe it's time to go outside and give your nerves a break.", "All precipitation and no success makes the Disciple a monotonous boy.", "You seem to be having a bit of trouble.  Maybe you should stop having trouble.", "This is easy.  That's why it's for immortal deities who're unable to die fully.", "All you need to do is fly across the map like a pinball.", "#JumpHigherDieLess" };
                    //Debug.LogWarning("NON-GENERATED: " + sentences[select]);
                    S = sentences[select];
                    last = select;
                    hasFallen = false;
                }
                else if (deathsDuringLevel % 10 == 0 && deathsDuringLevel >= 10)
                {
                    string[] sentences = { "ah, the monotony of death.", "you lose some, you lose some.", "Trying again is always an option.", "A winning move would be to avoid gameplay.", "By all means, continue.", "There's more death on its way, unfortunately.", "The futility of existence pesters us all.", "There's another ten.  Having problems with your footing?", "I'm sure you feel you've died enough.", "A death a minute keeps the sanity away.", "Who needs life when you have the ground?", "Acceptance or self-hatred?" };
                    //Debug.LogWarning("NON-GENERATED: " + sentences[select]);
                    S = sentences[select];
                    last = select;
                    hasFallen = false;
                }
                else if (inAir > 55)
                {
                    string[] sentences = { "tubular.", "what a neat aerial assassination on yourself.", "Banzaiiiii...", "Chill, Bill Murray in Groundhog Day.", "I grant you 10,000 points for that jump.  Points are worthless.", "You're not going to bounce.", "Be nice to your skeleton.", "You broke the fall with yourself.", "You are as graceful as a meteor.", "I feel worse for the floor than I do for you.", "You've shown the ground who's boss.", "Praise the ground." };
                    //Debug.LogWarning("NON-GENERATED: " + sentences[select]);
                    S = sentences[select];
                    last = select;
                    hasFallen = false;
                }
                else if (distFromGoal > 125)
                {
                    string[] sentences = { "what a great beginning.", "you're off to a good start.", "wonderful.", "At least fall later.", "There are other platforms to miss.", "You needn't miss the first few platforms, honestly.", "If you can't deal with the drops, you can't deal with the bad guys.", "Deal with the drops to deal with the bad guys.", "Come on.", "You didn't even try.", "You're really coordinated.", "How did you turn on the computer you're using?" };
                    //Debug.LogWarning("NON-GENERATED: " + sentences[select]);
                    S = sentences[select];
                    last = select;
                    hasFallen = false;
                }
                else if (distFromGoal > 60 && distFromGoal < 125)
                {
                    string[] sentences = { "you could be doing worse.", "at least you've experienced the level.", "This is a stupid level, anyway.", "Oh, what difference does it make?", "Another death for the collection.", "That'll leave several marks, including emotional ones.", "You'll be fine, even if that happens a few more times.", "Okay, so, you died.  We've all been there.", "You were doing kind of well.", "Death annoys you, I'm sure.  It annoys me, too.", "Decent progress.", "Nope." };
                    //Debug.LogWarning("NON-GENERATED: " + sentences[select]);
                    S = sentences[select];
                    last = select;
                    hasFallen = false;
                }
                else if (distFromGoal < 60)
                {
                    string[] sentences = { "brutal.", "gravity's not a nice person.", "What a lamentable trajectory.", "You were off by a little bit.", "Woosh.", "Lives could be worse.", "You've died so close to the end.  Neat.", "We were overdue for a death.", "Wow.", "You must have done something very bad to deserve this.", "This is what being immediately above par feels like.", "You embarked on quite a trek, before death.  Now, you can do it, again." };
                    //Debug.LogWarning("NON-GENERATED: " + sentences[select]);
                    S = sentences[select];
                    last = select;
                    hasFallen = false;
                }
            }

            else if (quickSuccessionStabs)
            {
                if (boltUnusedForLong)
                {
                    string[] sentences = { "escapism works.", "fear not.  you can bolt.", "Getting stabbed is intensely overrated.", "From bolt's absence comes cuts and bruises.  Who knew?", "'Pie jesu domine,' *shank* 'Dona peis requiem,' *shank*", "Slowpoke.", "If you get stabbed enough, you pretty much just become a donut or a bagel.  No bolting, but rolling, which works as well.", "If only you bolted as often as they stabbed you.", "Bolt past them.", "Use bolt to dodge their blows.", "Stabbing a moving target is harder.", "Concept: Move fast to kill fast." };
                    //Debug.LogWarning("NON-GENERATED: " + sentences[select]);
                    S = sentences[select];
                    last = select;
                    quickSuccessionStabs = false;
                    hasBeenStabbed = false;
                    boltUnusedForLong = false;
                }
                else if (distFromGoal > 125)
                {
                    string[] sentences = { "the AOL guy says, 'You got stabbed.'", "in the beginning, there were knives.", "Access intensely denied.", "These guys could have given you a little more time.", "Already, the stabbing begins.", "They should be stabbing things their own size.", "An immediate debacle.", "Get stabbed fast to succeed.", "Surprise swords.", "Swords don't let you come very close.", "Cowabunga.", "Do you even know how to breathe?" };
                    //Debug.LogWarning("NON-GENERATED: " + sentences[select]);
                    S = sentences[select];
                    last = select;
                    quickSuccessionStabs = false;
                    hasBeenStabbed = false;
                }
                else if (distFromGoal > 60 && distFromGoal < 125)
                {
                    string[] sentences = { "i appreciate your ability to maintain your current rate of being stabbed.", "Practice makes Polonius.", "At least you could be getting stabbed more often.", "Mr. Hyuga delivers his otherwise quick blows at a slower pace.", "The stabbings are spaced apart so well.  It's like a soap opera.", "If you're offering Morse Code signals, what's a dot, and what's a dash?", "As entertaining as these murders are, they seem ineffective.", "Something's not working.", "You were doing kind of well for a guy who likes to be stabbed.", "Those abysmal swords... always stabbing things.", "Possibly decent progress.", "The swords decline." };
                    //Debug.LogWarning("NON-GENERATED: " + sentences[select]);
                    S = sentences[select];
                    last = select;
                    quickSuccessionStabs = false;
                    hasBeenStabbed = false;
                }
                else
                {
                    string[] sentences = { "there's never a dull moment in the Infinite Domain.", "Way to dull some blades.", "You were just distracting them.  Of course!", "Feel free to stop getting stabbed.", "I dare you not to be stabbed, again.", "Do what you want.", "Getting stabbed isn't that interesting.", "Being stabbed is not the entire point of the game.", "Stabpocalypse.", "Come on.  Stop getting stabbed.", "Do you want this to happen to you?", "What gives with all the knives?" };
                    //Debug.LogWarning("NON-GENERATED: " + sentences[select]);
                    S = sentences[select];
                    last = select;
                    quickSuccessionStabs = false;
                    hasBeenStabbed = false;
                }
            }

            else if (hasBeenStabbed)
            {
                if (boltUnusedForLong)
                {
                    string[] sentences = { "bolt might be of assistance.", "Don't get stabbed.  Use bolt, instead.", "Go a little faster.  Make your life easier.", "Well, you haven't used bolt in a while.", "You can press the 'shift' key to go faster.", "You could have avoided that.", "Go.  Don't loiter.", "Standing still is punishable by death.", "Bolt past them.", "Use bolt to dodge their blows.", "Stabbing a moving target is harder.", "Concept: Move fast to kill fast." };
                    //Debug.LogWarning("NON-GENERATED: " + sentences[select]);
                    S = sentences[select];
                    last = select;
                    hasBeenStabbed = false;
                    boltUnusedForLong = false;
                }
                else if (deathsDuringLevel % 60 == 0 && deathsDuringLevel >= 60)
                {
                    string[] sentences = { "knives yield monotony.", "Knives can be upsetting, can't they?", "Julius Caesar?  Is that you?", "So many stabs.  So little diversity.", "That's a lot of murder.", "What difference does another death make?", "How many stabs could a Disciple receive if a Disciple could occasionally avoid stabs?", "A few evasive maneuvers wouldn't hurt.", "The abundance of weapons isn't making things easier for you.", "Dying is tough, especially when you get stabbed.", "Surely, you're used to this, by now.", "I'm not sure I understand your deep infatuation with death." };
                    //Debug.LogWarning("NON-GENERATED: " + sentences[select]);
                    S = sentences[select];
                    last = select;
                    hasBeenStabbed = false;
                }
                else if (deathsDuringLevel % 10 == 0 && deathsDuringLevel >= 10)
                {
                    string[] sentences = { "i suppose there could be more stabs.", "It's okay.  I don't even have a body worth stabbing.", "Maybe you really are a swarma, after all.", "You're still a pretty elusive swarma, at least.", "You've been stabbed.  Happens to the best of us.", "It's time for a change of behavior.", "Maybe you should change your behavior.", "Recall Einstein's definition of insanity.", "What a way to go.", "A death a minute keeps the sanity away.", "Who needs life when you have steel?", "Hooray for knives." };
                    //Debug.LogWarning("NON-GENERATED: " + sentences[select]);
                    S = sentences[select];
                    last = select;
                    hasBeenStabbed = false;
                }
                else if (inAir > 55)
                {
                    string[] sentences = { "an interceptor.", "good job, bad guy.", "I'm surprised one of them was able to do that.", "Maybe these enemies aren't so dumb.", "I could have sworn these guys were dumber.", "They shouldn't have managed that.", "I can't believe they did that.", "An enemy with that level of precision is not okay.", "Interception.", "They've saved you from the clutches of the ground.", "Sweet air makes for catastrophic collisions.  Tradeoffs.", "Aerial murder, shmaerial murder." };
                    //Debug.LogWarning("NON-GENERATED: " + sentences[select]);
                    S = sentences[select];
                    last = select;
                    hasBeenStabbed = false;
                }
                else if (distFromGoal > 125)
                {
                    string[] sentences = { "Well, then...", "The enemies' minds are made up.", "The level ends as the level starts: abruptly, and without cutscenes.", "That's too bad.", "Shame.", "Doing great, already.", "Would you look at that?", "I'm sad and surprised, actually.", "Surprise swords.", "Swords don't let you come very close.", "Cowabunga.", "Do you even know how to breathe?" };
                    //Debug.LogWarning("NON-GENERATED: " + sentences[select]);
                    S = sentences[select];
                    last = select;
                    hasBeenStabbed = false;
                }
                else if (distFromGoal > 60 && distFromGoal < 125)
                {
                    string[] sentences = { "Not the best time to be stabbed.", "There are better times at which to be stabbed.", "Looks like it's going to be one of those days.", "True ambition is rebounding from impertinent labor.", "Being stabbed halfway through a level builds character.", "Sometimes, the most ambitious person must settle for character building.", "Perhaps you've died with style.", "I guess you could be dying a little more often.", "You were doing kind of well for a guy who likes to be stabbed.", "Those abysmal swords... always stabbing things.", "Possibly decent progress.", "The swords decline." };
                    //Debug.LogWarning("NON-GENERATED: " + sentences[select]);
                    S = sentences[select];
                    last = select;
                    hasBeenStabbed = false;
                }
                else if (distFromGoal < 60)
                {
                    string[] sentences = { "Yahtzee.", "So close, yet so skewered.", "Looking like a turnstile at the end of the level.  Sheesh.", "Yeah, that's not good.", "That's a far cry from good.", "That's the opposite of good.", "Wouldn't it have been nice to be stabbed at any other time?", "Wrong place, wrong time.  Enemies are mean.", "My word.", "Venture to the pentagram.  Stray from enemies.", "Your ultimate success would be incomplete without another death.", "What a buzzkill." };
                    //Debug.LogWarning("NON-GENERATED: " + sentences[select]);
                    S = sentences[select];
                    last = select;
                    hasBeenStabbed = false;
                }
            }

            else if (hasKilledBoss)
            {
                if (boltUnusedForLong)
                {
                    string[] sentences = { "Bolt might've helped you, but you seem to have prevailed, anyway.", "Bolt might've made things easier.", "Giving yourself more of a challenge, by abstaining from bolt, I see.", "Oh you could have done that a little faster.", "Don't forget that bolt helps you kill stuff faster.", "You could have killed the boss faster with bolt.", "You could have spiced up that kill with a bolt.", "Aw. You didn't bolt.", "Without help from bolt.", "You didn't need very much help with that at all.", "That's a pretty legit strike from the heavens and whatnot, and you didn't have to use bolt.", "You're making some kind of point, perhaps." };
                    //Debug.LogWarning("NON-GENERATED: " + sentences[select]);
                    S = sentences[select];
                    last = select;
                    hasKilledBoss = false;
                    hasStabbed = false;
                    boltUnusedForLong = false;
                }
                else if (aerialKill)
                {
                    string[] sentences = { "Acrobatics!", "Killing bosses is such good exercise.", "A boss-defying leap.", "What a spectacular way to kill something.", "That was pretty cool.", "Show off.", "You're just showing off.", "There you go.", "Now, you're the boss.", "Elegant.", "That's a pretty legit strike from the heavens and whatnot.", "A spectacle." };
                    //Debug.LogWarning("NON-GENERATED: " + sentences[select]);
                    S = sentences[select];
                    last = select;
                    hasKilledBoss = false;
                    hasStabbed = false;
                }
            }

            else if (hasStabbed)
            {
                if (boltUnusedForLong)
                {
                    string[] sentences = { "You could kill even more enemies if you used bolt.", "Bolt might yield additional kills.", "You could kill more enemies with bolt.", "Bolt could help you kill a ton of enemies.", "Killing enemies might be even easier for you if you throw in a bolt every now and then.", "Bolt would help you kill the bad guys.", "Swords are cool, but bolt makes the bad guys go away, too.", "Use bolt.  Kill faster.", "And without using bolt for a while.  Impressive.", "Look, ma. No bolting.", "If you used bolt, you'd actually be doing better.", "Using bolt might allow you to kill more." };
                    //Debug.LogWarning("NON-GENERATED: " + sentences[select]);
                    S = sentences[select];
                    last = select;
                    hasStabbed = false;
                }
                else if (deathsDuringLevel % 10 == 0 && deathsDuringLevel >= 10)
                {
                    string[] sentences = { "Look who's making a comeback.", "Now, you're angry.", "That's it. Become a menace.", "Teach those ones and zeroes who's boss.", "Disciple smash!", "Give them the old two piece and a biscuit.", "How the turns have tabled.", "How the tables have turned.", "You're making a comeback.", "Keep at the killing.", "A quality kill.", "Well, look at that." };
                    //Debug.LogWarning("NON-GENERATED: " + sentences[select]);
                    S = sentences[select];
                    last = select;
                    hasStabbed = false;
                }
                else
                {
                    string[] sentences = { "Tango down.", "That was pretty nice.", "Word.", "Hello, sword.", "Eat swords, filth.", "Solid swing.", "Bonk.", "Hya.", "Et tu?", "Well, aren't you a foolish samurai warrior wielding a magic sword?", "I see, you like a good kebab.", "Like a glove." };
                    //Debug.LogWarning("NON-GENERATED: " + sentences[select]);
                    S = sentences[select];
                    last = select;
                    hasStabbed = false;
                }
            }

            else if (boltMeterEmpty)
            {
                if (quickSuccessionFalls)
                {
                    string[] sentences = { "Slow down.", "There's such a thing as using bolt too often.", "Use bolt, but only for a limited time.", "Bolting helps the patient.", "Patience is necessary for proper bolting.", "Energy depletes quickly.", "Don't do everything too fast.", "Give 'shift' a break.", "You're falling like crazy.  Bolt could assist you.", "Stop bolting. Allow for a recharge.", "Don't tire yourself out.", "You can't run like that, forever." };
                    //Debug.LogWarning("NON-GENERATED: " + sentences[select]);
                    S = sentences[select];
                    last = select;
                    boltMeterEmpty = false;
                }
                else if (hasBeenStabbed)
                {
                    string[] sentences = { "Give yourself enough energy to bolt out of there.", "Make sure you have enough energy for an escape.", "Escaping is a lot harder without bolt.", "Not enough 'juice.'", "Give bolt some time to charge before you need it.", "Try bolting out of that one, again.", "Be careful about your energy.", "Mind your energy.", "You haven't enough 'juice,' as they say.", "Stop bolting. Allow for a recharge.", "Don't tire yourself out.", "You can't run like that, forever." };
                    //Debug.LogWarning("NON-GENERATED: " + sentences[select]);
                    S = sentences[select];
                    last = select;
                    boltMeterEmpty = false;
                }
                else if (hasStabbed)
                {
                    string[] sentences = { "What a swift kill.", "A masterful use of the bolt ability.", "Nice bolt.", "A brilliant thrust.", "That's a good shot.", "Quality hit.", "Good one!", "You got 'em!", "You seem to prevail without using bolt.", "Stop bolting. Allow for a recharge.", "Don't tire yourself out.", "You can't run like that, forever." };
                    //Debug.LogWarning("NON-GENERATED: " + sentences[select]);
                    S = sentences[select];
                    last = select;
                    boltMeterEmpty = false;
                }
                else if (quickSuccessionStabs)
                {
                    string[] sentences = { "you'd be stabbed a bit less if you used bolt properly.", "stop bolting. Allow for a recharge.", "Don't tire yourself out.", "You can't run like that, forever." };
                    //Debug.LogWarning("NON-GENERATED: " + sentences[select]);
                    S = sentences[select];
                    last = select;
                    boltMeterEmpty = false;
                }
                else
                {
                    boltMeterEmpty = false;
                }
            }
        }
        else {
            if (hasFallen)
            {
                temp = Random.Range(7, 8);
                tempObj = 11;
                tenseSwitch = Random.Range(0, 3);
            }
            else if (hasBeenStabbed)
            {
                temp = Random.Range(0, 1);
                tempObj = 11;
                tenseSwitch = Random.Range(0, 3);
            }
            else if (hasStabbed)
            {
                temp = 11;
                tempObj = Random.Range(0, 1);
                tenseSwitch = Random.Range(0, 3);
            }
            else
            {
                return;
            }
            int articleSwitch = Random.Range(0, 1); //Choose whether to use certain articles or not.
            int articleSwitchObj = Random.Range(0, 1); //Choose whether to use certain articles with regard to the object.
            int usePronoun = Random.Range(0, 1); //Determine whether to have a pronoun be the subject.
            int usePronounObj = Random.Range(0, 1); //Determine whether to have a pronoun be the object.

            string NounVar;

            if (singularOrPlural == 0 && (temp <= 12 || temp >= 20))
            {
                NounVar = nounArray[temp].getSingular();
            }

            else
            {
                NounVar = nounArray[temp].getPlural();
            }

            string Object;

            if (singularOrPluralObj == 0 && (tempObj <= 12 || tempObj >= 20))
            {
                Object = nounArray[tempObj].getSingular();
                if (hasFallen || hasBeenStabbed || hasStabbed)
                {
                    Object = nounArray[tempObj].getSingular();
                }
            }

            else
            {
                Object = nounArray[tempObj].getPlural();
            }

            string VerbVar = "";
            int verbChoice = 0;
            if (hasFallen)
            {
                verbChoice = 0;
            }
            else if (hasBeenStabbed || hasStabbed)
            {
                verbChoice = Random.Range(8, 10);
            }
            switch (tenseSwitch)
            {
                case 0:
                    VerbVar = verbArray[verbChoice].getPast();
                    break;

                case 1:
                    if (singularOrPlural == 0)
                    {
                        VerbVar = verbArray[verbChoice].getTpSingular();
                    }
                    else
                    {
                        VerbVar = verbArray[verbChoice].getRoot();
                    }
                    break;



                case 2:
                    if (singularOrPlural == 0 && (temp <= 12 || temp >= 20))
                    {
                        VerbVar = "has " + verbArray[verbChoice].getPastParticiple();
                    }
                    else
                    {
                        VerbVar = "have " + verbArray[verbChoice].getPastParticiple();
                    }
                    break;

                case 3:
                    VerbVar = "had " + verbArray[Random.Range(0, 16)].getPastParticiple();
                    break;

                case 4:
                    VerbVar = "will " + verbArray[Random.Range(0, 16)].getRoot();
                    break;
            }

            VP = VerbVar;

            if (nounArray[temp].getIsProper())
            {
                if (temp >= 12 && temp <= 20)
                {
                    NP = NounVar;
                }

                else
                {
                    NP = articles[2] + " " + NounVar;
                }
            }

            else
            {
                if (articleSwitch == 1)
                {
                    NP = articles[Random.Range(2, 6)] + " " + NounVar;
                }

                if (singularOrPlural == 1)
                {
                    NP = articles[Random.Range(7, 10)] + " " + NounVar;
                }

                else
                {
                    if (nounArray[temp].getBeginsWithVowel())
                    {
                        NP = articles[0] + " " + NounVar;
                    }

                    else
                    {
                        NP = articles[1] + " " + NounVar;
                    }

                    if (usePronoun == 1)
                    {
                        NP = subjectPronouns[Random.Range(0, 3)];
                        if (singularOrPlural == 1)
                        {
                            NP = subjectPronouns[Random.Range(4, 7)];
                        }
                    }
                }
            }

            if (nounArray[tempObj].getIsProper())
            {
                if (tempObj >= 12 && tempObj <= 20)
                {
                    NPObj = Object;
                }

                else
                {
                    NPObj = articles[2] + " " + Object;
                }
            }

            else
            {
                if (articleSwitchObj == 1)
                {
                    NPObj = articles[Random.Range(2, 6)] + " " + Object;
                }

                if (singularOrPluralObj == 1)
                {
                    NPObj = articles[Random.Range(7, 10)] + " " + Object;
                }

                else
                {
                    if (nounArray[tempObj].getBeginsWithVowel())
                    {
                        NPObj = articles[0] + " " + Object;
                    }

                    else
                    {
                        NPObj = articles[1] + " " + Object;
                    }

                    if (usePronounObj == 1)
                    {
                        NPObj = objectPronouns[Random.Range(0, 7)];
                    }
                }
            }

            hasFallen = false;
            quickSuccessionFalls = false;
            hasBeenStabbed = false;
            quickSuccessionStabs = false;
            boltMeterEmpty = false;
            hasKilledBoss = false;
            boltUnusedForLong = false;
            aerialKill = false;
            hasStabbed = false;

            S = NP + " " + VP + " " + NPObj + ".";
            //Debug.LogWarning("articleSwitch: " + articleSwitch + " Sentence: " + S);
        }
    }
}