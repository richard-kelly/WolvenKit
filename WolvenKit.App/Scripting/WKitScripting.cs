using System;
using System.Collections.Generic;
using System.IO;
using Splat;
using WolvenKit.Common.Services;
using WolvenKit.Functionality.Services;
using WolvenKit.ProjectManagement.Project;
using WolvenKit.RED4.Archive.CR2W;
using WolvenKit.RED4.Archive.IO;
using WolvenKit.RED4.Types;

namespace WolvenKit.Scripting;

public class WKitScripting
{
    private readonly IProjectManager _projectManager;
    private readonly ILoggerService _loggerService;

    private readonly List<string> _catFacts = new()
    {
        "A house cat’s genome is 95.6 percent tiger, and they share many behaviors with their jungle ancestors, says Layla Morgan Wilde, a cat behavior expert and the founder of Cat Wisdom 101. These behaviors include scent marking by scratching, prey play, prey stalking, pouncing, chinning, and urine marking.",
        "Cats are believed to be the only mammals who don’t taste sweetness.",
        "Cats are nearsighted, but their peripheral vision and night vision are much better than that of humans.",
        "Cats are supposed to have 18 toes (five toes on each front paw; four toes on each back paw).",
        "Cats can jump up to six times their length.",
        "Cats’ claws all curve downward, which means that they can’t climb down trees head-first. Instead, they have to back down the trunk.",
        "Cats’ collarbones don’t connect to their other bones, as these bones are buried in their shoulder muscles.",
        "Cats have 230 bones, while humans only have 206.",
        "Cats have an extra organ that allows them to taste scents on the air, which is why your cat stares at you with her mouth open from time to time.",
        "Cats have whiskers on the backs of their front legs, as well.",
        "Cats have nearly twice the amount of neurons in their cerebral cortex as dogs.",
        "Cats have the largest eyes relative to their head size of any mammal.",
        "Cats make very little noise when they walk around. The thick, soft pads on their paws allow them to sneak up on their prey — or you!",
        "Cats’ rough tongues can lick a bone clean of any shred of meat.",
        "Cats use their long tails to balance themselves when they’re jumping or walking along narrow ledges.",
        "Cats use their whiskers to “feel” the world around them in an effort to determine which small spaces they can fit into. A cat’s whiskers are generally about the same width as its body. (This is why you should never, EVER cut their whiskers.)",
        "Cats walk like camels and giraffes: They move both of their right feet first, then move both of their left feet. No other animals walk this way.",
        "Male cats are more likely to be left-pawed, while female cats are more likely to be right-pawed.",
        "Though cats can notice the fast movements of their prey, it often seems to them that slow-moving objects are actually stagnant.",
        "Some cats are ambidextrous, but 40 percent are either left- or right-pawed.",
        "Some cats can swim.",
        "There are cats who have more than 18 toes. These extra-digit felines are referred to as being “polydactyl.”"
    };

    public WKitScripting()
    {
        _projectManager = Locator.Current.GetService<IProjectManager>();
        _loggerService = Locator.Current.GetService<ILoggerService>();
    }

    public void Info(string message)
    {
        _loggerService.Info(message);
    }

    public void Error(string message)
    {
        _loggerService.Error(message);
    }

    public void RandomCatFact()
    {
        var rnd = new Random();
        Info(_catFacts[rnd.Next(0, _catFacts.Count - 1)]);
    }

    public CR2WFile OpenFile(string path)
    {
        if (_projectManager.ActiveProject is not Cp77Project cp77)
        {
            return null;
        }

        if (!cp77.ModFiles.Contains(path))
        {
            // Import from archive? Ignore for now
            return null;
        }

        var fullPath = Path.Combine(cp77.ModDirectory, path);

        using var ms = new MemoryStream(File.ReadAllBytes(fullPath));
        using var reader = new CR2WReader(ms);

        if (reader.ReadFile(out var cr2w) != EFileReadErrorCodes.NoError)
        {
            return null;
        }

        cr2w.MetaData.FileName = path;

        return cr2w;
    }

    public void SaveFile(CR2WFile file, string path = null)
    {
        path ??= file.MetaData.FileName;

        if (_projectManager.ActiveProject is not Cp77Project cp77)
        {
            return;
        }

        var fullPath = Path.Combine(cp77.ModDirectory, path);

        using var ms = new MemoryStream();
        using var writer = new CR2WWriter(ms);

        writer.WriteFile(file);

        File.WriteAllBytes(fullPath, ms.ToArray());
    }

    public void ReplacePath(CR2WFile file, string oldStr, string newStr)
    {
        var targetType = typeof(IRedRef);

        foreach (var tuple in file.RootChunk.GetEnumerator())
        {
            if (targetType.IsInstanceOfType(tuple.value))
            {
                var value = (IRedRef)tuple.value;
                if (oldStr == "*" || (string)value.DepotPath == oldStr)
                {
                    value.DepotPath = newStr;
                }
            }
        }
    }
}
