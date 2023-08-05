using UnityEngine;
using System.Text.RegularExpressions;

public class EmailValidation : MonoBehaviour
{
    //public string emailToValidate = "test@example.com";

    public static void ValidateEmail(string email)
    {
        bool isValid = IsValidEmail(email);
        if (isValid)
        {
            Debug.Log("Email is valid.");
        }
        else
        {
            Debug.Log("Email is not valid.");
        }
    }

    public static bool IsValidEmail(string email)
    {
        // Regular expression pattern for email validation
        string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";

        // Use Regex.IsMatch to check if the email matches the pattern
        return Regex.IsMatch(email, pattern);
    }
}
