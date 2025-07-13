using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using PsychologicalTestsApp.Models;

public class DatabaseHelper
{
    private string connectionString = "Server=localhost;Database=PsychologicalTests;User ID=root;Password=;";

    public User GetUser(string username, string password)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var command = new MySqlCommand("SELECT * FROM Accounts WHERE username = @username AND password = @password", connection);
            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@password", password);

            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new User
                    {
                        Id = reader.GetInt32("id"),
                        Username = reader.GetString("username"),
                        Password = reader.GetString("password"),
                        IsTeacher = true
                    };
                }
            }
        }
        return null;
    }

    public List<Test> GetTestsByTheme(string themeName)
    {
        List<Test> tests = new List<Test>();
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string query = @"
        SELECT t.id, t.name 
        FROM Tests t
        JOIN Themes th ON t.theme_id = th.id
        WHERE th.name = @themeName";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@themeName", themeName);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tests.Add(new Test
                        {
                            Id = reader.GetInt32("id"),
                            Name = reader.GetString("name")
                        });
                    }
                }
            }
        }
        return tests;
    }


    public List<Answer> GetAnswersByQuestion(int questionId)
    {
        List<Answer> answers = new List<Answer>();
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var command = new MySqlCommand(
                "SELECT id, text FROM Answers WHERE question_id = @questionId", connection);
            command.Parameters.AddWithValue("@questionId", questionId);

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    answers.Add(new Answer
                    {
                        Id = reader.GetInt32("id"),
                        Text = reader.GetString("text"),
                        QuestionId = questionId
                    });
                }
            }
        }
        return answers;
    }

    public string GetResultDescription(int score)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var command = new MySqlCommand(
                "SELECT description FROM TestResultsDescriptions WHERE @score BETWEEN min_score AND max_score", connection);
            command.Parameters.AddWithValue("@score", score);

            object result = command.ExecuteScalar();
            return result != null ? result.ToString() : "Результат не определен.";
        }
    }
    public bool IsCorrectAnswer(int answerId)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var command = new MySqlCommand(
                "SELECT COUNT(*) FROM `Keys` WHERE answer_id = @answerId", connection);
            command.Parameters.AddWithValue("@answerId", answerId);

            int count = Convert.ToInt32(command.ExecuteScalar());
            return count > 0;
        }
    }


    public bool SaveTestResult(int studentId, int testId, int score)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var command = new MySqlCommand(
                "INSERT INTO TestResults (student_id, test_id, score) " +
                "VALUES (@studentId, @testId, @score)", connection);
            command.Parameters.AddWithValue("@studentId", studentId);
            command.Parameters.AddWithValue("@testId", testId);
            command.Parameters.AddWithValue("@score", score);

            return command.ExecuteNonQuery() > 0;
        }
    }

    public int GetTestIdByName(string testName)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var command = new MySqlCommand(
                "SELECT id FROM Tests WHERE name = @testName", connection);
            command.Parameters.AddWithValue("@testName", testName);

            object result = command.ExecuteScalar();
            return result != null ? Convert.ToInt32(result) : -1;
        }
    }

    public List<Question> GetQuestionsByTest(string testName)
    {
        List<Question> questions = new List<Question>();
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string query = @"
            SELECT q.id, q.text 
            FROM Questions q
            JOIN Tests t ON q.test_id = t.id
            WHERE t.name = @testName";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@testName", testName);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        questions.Add(new Question
                        {
                            Id = reader.GetInt32("id"),
                            Text = reader.GetString("text")
                        });
                    }
                }
            }
        }
        return questions;
    }

    public List<Test> GetTests()
    {
        List<Test> tests = new List<Test>();
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string query = "SELECT * FROM Tests";
            using (var command = new MySqlCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tests.Add(new Test
                        {
                            Id = reader.GetInt32("id"),
                            Name = reader.GetString("name")
                        });
                    }
                }
            }
        }
        return tests;
    }

    public bool GroupExists(string groupName)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var command = new MySqlCommand("SELECT COUNT(*) FROM `Groups` WHERE name = @groupName", connection);
            command.Parameters.AddWithValue("@groupName", groupName);

            int count = Convert.ToInt32(command.ExecuteScalar());
            return count > 0;
        }
    }

    public List<Student> GetStudents()
    {
        var students = new List<Student>();
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var command = new MySqlCommand("SELECT * FROM Students", connection);
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    students.Add(new Student
                    {
                        Id = reader.GetInt32("id"),
                        Name = reader.GetString("name"),
                        GroupId = reader.GetInt32("group_id")
                    });
                }
            }
        }
        return students;
    }

    public Student GetStudent(string fullName, string groupName)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var command = new MySqlCommand(@"
            SELECT s.* FROM Students s
            JOIN `Groups` g ON s.group_id = g.id
            WHERE s.name = @fullName AND g.name = @groupName", connection);
            command.Parameters.AddWithValue("@fullName", fullName);
            command.Parameters.AddWithValue("@groupName", groupName);

            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new Student
                    {
                        Id = reader.GetInt32("id"),
                        Name = reader.GetString("name"),
                        GroupId = reader.GetInt32("group_id")
                    };
                }
            }
        }
        return null;
    }


    public List<string> GetThemes()
    {
        List<string> themes = new List<string>();
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string query = "SELECT name FROM Themes";
            using (var command = new MySqlCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        themes.Add(reader.GetString(0));
                    }
                }
            }
        }
        return themes;
    }

    public bool AddGroup(string groupName)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var command = new MySqlCommand("INSERT INTO `Groups` (name) VALUES (@groupName)", connection);
            command.Parameters.AddWithValue("@groupName", groupName);

            return command.ExecuteNonQuery() > 0;
        }
    }



    public List<string> GetGroups()
    {
        List<string> groups = new List<string>();
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string query = "SELECT name FROM `Groups`"; // Обратите внимание на обратные кавычки
            using (var command = new MySqlCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        groups.Add(reader.GetString(0));
                    }
                }
            }
        }
        return groups;
    }


    public bool AddStudent(string fullName, string groupName)
    {
        // Сначала получаем ID группы по имени
        int groupId = GetGroupIdByName(groupName);
        if (groupId == -1)
        {
            return false; // Группа не найдена
        }

        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var command = new MySqlCommand("INSERT INTO Students (name, group_id) VALUES (@fullName, @groupId)", connection);
            command.Parameters.AddWithValue("@fullName", fullName);
            command.Parameters.AddWithValue("@groupId", groupId);

            return command.ExecuteNonQuery() > 0;
        }
    }

    private int GetGroupIdByName(string groupName)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var command = new MySqlCommand("SELECT id FROM `Groups` WHERE name = @groupName", connection); // Обратите внимание на обратные кавычки
            command.Parameters.AddWithValue("@groupName", groupName);

            object result = command.ExecuteScalar();
            return result != null ? Convert.ToInt32(result) : -1;
        }
    }





    public List<string> GetDetailsByTheme(string themeName)
    {
        List<string> details = new List<string>();
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string query = @"
                SELECT b.name FROM Blocks b
                JOIN Themes t ON b.theme_id = t.id
                WHERE t.name = @themeName";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@themeName", themeName);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        details.Add(reader.GetString(0));
                    }
                }
            }
        }
        return details;
    }
}