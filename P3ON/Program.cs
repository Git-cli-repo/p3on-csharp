using System.Runtime.Serialization;
using P3ONCore;
using System.Collections.Generic;

string filepath = "C:/Users/rudra/p3on.proj/P3ON/myfile_.p3on";
Dictionary<string, object> TokenData = P3ON.Read(filepath);

Console.WriteLine($"TokenData count: {TokenData.Count}");

if (TokenData.ContainsKey("myjnode") && TokenData["myjnode"] is Dictionary<string, object> myJNode)
{
    if (myJNode.ContainsKey("key") && myJNode["key"] is List<int> keyList)
    {
        Console.WriteLine($"Array immediately after read: {string.Join(", ", keyList)}");
    }
}

foreach (KeyValuePair<string, object> token in TokenData)
{
    Console.WriteLine($"Key: {token.Key}, Value: {token.Value}, Value type: {token.Value?.GetType()}");
}

Console.WriteLine(TokenData["myjnode"]);
Dictionary<string, object> MyJNode = (Dictionary<string, object>)TokenData["myjnode"];
List<int> Key = (List<int>)MyJNode["key"];
Console.WriteLine(Key.Count);
MyJNode["key"] = Key;
TokenData["myjnode"] = MyJNode;
P3ON.Write(TokenData, "C:/Users/rudra/p3on.proj/P3ON/myfile_-edited.p3on");


string _filepath = "C:/Users/rudra/p3on.proj/P3ON/myfile.p3on";

// Read the P3ON data
Dictionary<string, object> _TokenData = P3ON.Read(_filepath);

// Access and modify the data
if (_TokenData.ContainsKey("userData") && _TokenData["userData"] is Dictionary<string, object> userData)
{
    if (userData.ContainsKey("profile") && userData["profile"] is Dictionary<string, object> profile)
    {
        // Change the username
        if (profile.ContainsKey("username"))
        {
            profile["username"] = "NewUser";
        }

        // Increase the age
        if (profile.ContainsKey("age") && profile["age"] is int age)
        {
            profile["age"] = age + 1;
        }

        // Add a new interest
        if (profile.ContainsKey("interests") && profile["interests"] is List<string> interests)
        {
            interests.Add("Cooking");
            profile["interests"] = interests; // Update the list
        }

        //Change isActive
        if(profile.ContainsKey("isActive") && profile["isActive"] is bool isActive){
            profile["isActive"] = !isActive;
        }

        //Add a new child Object
        Dictionary<string, object> newObject = new Dictionary<string, object>();
        newObject.Add("occupation", "Developer");
        profile.Add("job", newObject);

        // Update the profile in the userData dictionary
        userData["profile"] = profile;

        // Update userData in the _TokenData Dictionary.
        _TokenData["userData"] = userData;
    }

    // Write the modified data back to the file
    P3ON.Write(_TokenData, "C:/Users/rudra/p3on.proj/P3ON/myfile-edited.p3on");
    Console.WriteLine("userData.p3on file updated.");
}
else
{
    Console.WriteLine("userData section not found.");
}
