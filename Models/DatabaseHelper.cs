using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using PsychologicalTestsApp.Models;

public class DatabaseHelper
{
    private string connectionString = "Server=localhost;Database=PsychologicalTests;User ID=root;Password=;";
    private readonly string _connectionString = "Server=localhost;Database=PsychologicalTests;User ID=root;Password=;";

    public class TestStudentCount
    {
        public string TestName { get; set; }
        public int StudentCount { get; set; }
    }
    public List<TestStudentCount> GetUniqueStudentsByTheme(string themeName, DateTime dateFrom, DateTime dateTo)
    {
        var testResults = new List<TestStudentCount>();
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var command = new MySqlCommand(
                @"SELECT t.name, COUNT(DISTINCT tr.student_id) as student_count
                      FROM Tests t
                      LEFT JOIN TestResults tr ON tr.test_id = t.id
                      AND tr.completed_at BETWEEN @DateFrom AND @DateTo
                      JOIN Themes th ON t.theme_id = th.id
                      WHERE th.name = @ThemeName
                      GROUP BY t.id, t.name",
                connection);
            command.Parameters.AddWithValue("@ThemeName", themeName);
            command.Parameters.AddWithValue("@DateFrom", dateFrom);
            command.Parameters.AddWithValue("@DateTo", dateTo);

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    testResults.Add(new TestStudentCount
                    {
                        TestName = reader.GetString(0),
                        StudentCount = reader.GetInt32(1)
                    });
                }
            }
        }
        return testResults;
    }

   

    public List<StudentDisplay> FetchStudents1()
    {
        var students = new List<StudentDisplay>();
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var command = new MySqlCommand(
                "SELECT s.id, s.name AS name, s.group_id, g.name AS group_name " +
                "FROM Students s LEFT JOIN `Groups` g ON s.group_id = g.id", connection);
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    students.Add(new StudentDisplay
                    {
                        Id = reader.GetInt32("id"),
                        Name = reader.GetString("name"),
                        GroupId = reader.GetInt32("group_id"),
                        GroupName = reader.IsDBNull(reader.GetOrdinal("group_name")) ? "" : reader.GetString("group_name")
                    });
                }
            }
        }
        return students;
    }

    public int InsertStudent1(string name, int groupId)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var command = new MySqlCommand(
                "INSERT INTO Students (full_name, group_id) VALUES (@name, @groupId); SELECT LAST_INSERT_ID();",
                connection);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@groupId", groupId);
            try
            {
                return Convert.ToInt32(command.ExecuteScalar());
            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in InsertStudent1: {ex.Message}");
                return -1;
            }
        }
    }

    public bool UpdateStudent1(int studentId, string name, int groupId)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var command = new MySqlCommand(
                "UPDATE Students SET full_name = @name, group_id = @groupId WHERE id = @studentId",
                connection);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@groupId", groupId);
            command.Parameters.AddWithValue("@studentId", studentId);
            try
            {
                return command.ExecuteNonQuery() > 0;
            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in UpdateStudent1: {ex.Message}, StudentId: {studentId}");
                return false;
            }
        }
    }

    public bool DeleteStudent1(int studentId)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var command = new MySqlCommand("DELETE FROM Students WHERE id = @studentId", connection);
            command.Parameters.AddWithValue("@studentId", studentId);
            try
            {
                return command.ExecuteNonQuery() > 0;
            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in DeleteStudent1: {ex.Message}, StudentId: {studentId}");
                return false;
            }
        }
    }
    public bool UpdateGroup1(int groupId, string groupName)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var command = new MySqlCommand("UPDATE `Groups` SET name = @groupName WHERE id = @groupId", connection);
            command.Parameters.AddWithValue("@groupName", groupName);
            command.Parameters.AddWithValue("@groupId", groupId);
            try
            {
                return command.ExecuteNonQuery() > 0;
            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in UpdateGroup1: {ex.Message}, GroupId: {groupId}");
                return false;
            }
        }
    }
    public List<GroupDisplay> FetchGroups1()
    {
        var groups = new List<GroupDisplay>();
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var command = new MySqlCommand("SELECT id, name FROM `Groups`", connection);
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    groups.Add(new GroupDisplay
                    {
                        Id = reader.GetInt32("id"),
                        Name = reader.GetString("name")
                    });
                }
            }
        }
        return groups;
    }

    public int InsertGroup1(string groupName)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var command = new MySqlCommand("INSERT INTO `Groups` (name) VALUES (@groupName); SELECT LAST_INSERT_ID();", connection);
            command.Parameters.AddWithValue("@groupName", groupName);
            try
            {
                return Convert.ToInt32(command.ExecuteScalar());
            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in InsertGroup1: {ex.Message}");
                return -1;
            }
        }
    }

    public bool AddGroup1(string groupName)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var command = new MySqlCommand("INSERT INTO `Groups` (name) VALUES (@groupName)", connection);
            command.Parameters.AddWithValue("@groupName", groupName);
            return command.ExecuteNonQuery() > 0;
        }
    }

    public bool DeleteGroup1(int groupId)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var command = new MySqlCommand("DELETE FROM `Groups` WHERE id = @groupId", connection);
            command.Parameters.AddWithValue("@groupId", groupId);
            try
            {
                return command.ExecuteNonQuery() > 0;
            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in DeleteGroup1: {ex.Message}, GroupId: {groupId}");
                return false;
            }
        }
    }
    public bool AddKey1(int answerId)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var checkCommand = new MySqlCommand("SELECT COUNT(*) FROM Answers WHERE id = @answerId", connection);
            checkCommand.Parameters.AddWithValue("@answerId", answerId);
            if ((long)checkCommand.ExecuteScalar() == 0)
            {
                System.Diagnostics.Debug.WriteLine($"Answer ID {answerId} does not exist in Answers table.");
                return false;
            }

            var command = new MySqlCommand("INSERT INTO `Keys` (answer_id) VALUES (@answerId)", connection);
            command.Parameters.AddWithValue("@answerId", answerId);
            try
            {
                return command.ExecuteNonQuery() > 0;
            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in AddKey: {ex.Message}, AnswerId: {answerId}");
                return false;
            }
        }
    }
    public bool DeleteQuestionsByTestId(int testId)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var command = new MySqlCommand("DELETE FROM Questions WHERE test_id = @testId", connection);
            command.Parameters.AddWithValue("@testId", testId);
            try
            {
                return command.ExecuteNonQuery() >= 0; // >= 0, так как может не быть записей для удаления
            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in DeleteQuestionsByTestId: {ex.Message}, TestId: {testId}");
                return false;
            }
        }
    }

    public bool DeleteRangesByTestId(int testId)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var command = new MySqlCommand("DELETE FROM TestResultsDescriptions WHERE test_id = @testId", connection);
            command.Parameters.AddWithValue("@testId", testId);
            try
            {
                return command.ExecuteNonQuery() >= 0; // >= 0, так как может не быть записей для удаления
            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in DeleteRangesByTestId: {ex.Message}, TestId: {testId}");
                return false;
            }
        }
    }

    public List<Group> GetGroupsWithIds()
    {
        List<Group> groups = new List<Group>();
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var command = new MySqlCommand("SELECT id, name FROM `Groups` ORDER BY name", connection);
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    groups.Add(new Group
                    {
                        Id = reader.GetInt32("id"),
                        Name = reader.GetString("name")
                    });
                }
            }
        }
        return groups;
    }

    public Group GetGroupById(int id)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var command = new MySqlCommand("SELECT id, name FROM `Groups` WHERE id = @id", connection);
            command.Parameters.AddWithValue("@id", id);
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new Group
                    {
                        Id = reader.GetInt32("id"),
                        Name = reader.GetString("name")
                    };
                }
            }
        }
        return null;
    }

    public bool DeleteGroup(int id)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var command = new MySqlCommand("DELETE FROM `Groups` WHERE id = @id", connection);
            command.Parameters.AddWithValue("@id", id);
            try
            {
                return command.ExecuteNonQuery() > 0;
            }
            catch (MySqlException)
            {
                return false; // Обработка ограничений внешнего ключа
            }
        }
    }

    public bool UpdateGroup(int id, string name)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var command = new MySqlCommand("UPDATE `Groups` SET name = @name WHERE id = @id", connection);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@id", id);
            return command.ExecuteNonQuery() > 0;
        }
    }

    // Студенты
    public List<StudentDisplay> GetStudentsWithGroupNames()
    {
        List<StudentDisplay> students = new List<StudentDisplay>();
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var command = new MySqlCommand("SELECT s.id, s.name, g.name AS group_name FROM Students s JOIN `Groups` g ON s.group_id = g.id ORDER BY s.name", connection);
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    students.Add(new StudentDisplay
                    {
                        Id = reader.GetInt32("id"),
                        Name = reader.GetString("name"),
                        GroupName = reader.GetString("group_name")
                    });
                }
            }
        }
        return students;
    }

    public Student GetStudentById(int id)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var command = new MySqlCommand("SELECT id, name, group_id FROM Students WHERE id = @id", connection);
            command.Parameters.AddWithValue("@id", id);
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

    public string GetGroupNameById(int groupId)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var command = new MySqlCommand("SELECT name FROM `Groups` WHERE id = @groupId", connection);
            command.Parameters.AddWithValue("@groupId", groupId);
            return command.ExecuteScalar()?.ToString();
        }
    }

    public bool DeleteStudent(int id)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var command = new MySqlCommand("DELETE FROM Students WHERE id = @id", connection);
            command.Parameters.AddWithValue("@id", id);
            try
            {
                return command.ExecuteNonQuery() > 0;
            }
            catch (MySqlException)
            {
                return false; // Обработка ограничений внешнего ключа
            }
        }
    }

    public bool UpdateStudent(int id, string name, string groupName)
    {
        int groupId = GetGroupIdByName(groupName);
        if (groupId == -1) return false;

        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var command = new MySqlCommand("UPDATE Students SET name = @name, group_id = @groupId WHERE id = @id", connection);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@groupId", groupId);
            command.Parameters.AddWithValue("@id", id);
            return command.ExecuteNonQuery() > 0;
        }
    }

    // Тесты
    public List<TestDisplay> GetTestsWithThemes()
    {
        List<TestDisplay> tests = new List<TestDisplay>();
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var command = new MySqlCommand("SELECT t.id, t.name, th.name AS theme_name FROM Tests t JOIN Themes th ON t.theme_id = th.id ORDER BY t.name", connection);
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    tests.Add(new TestDisplay
                    {
                        Id = reader.GetInt32("id"),
                        Name = reader.GetString("name"),
                        ThemeName = reader.GetString("theme_name")
                    });
                }
            }
        }
        return tests;
    }

    public TestDisplay GetTestById(int id)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var command = new MySqlCommand("SELECT t.id, t.name, th.name AS theme_name FROM Tests t JOIN Themes th ON t.theme_id = th.id WHERE t.id = @id", connection);
            command.Parameters.AddWithValue("@id", id);
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new TestDisplay
                    {
                        Id = reader.GetInt32("id"),
                        Name = reader.GetString("name"),
                        ThemeName = reader.GetString("theme_name")
                    };
                }
            }
        }
        return null;
    }

    public List<QuestionDisplay> GetQuestionsByTestId(int testId)
    {
        List<QuestionDisplay> questions = new List<QuestionDisplay>();
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var command = new MySqlCommand("SELECT id, text FROM Questions WHERE test_id = @testId", connection);
            command.Parameters.AddWithValue("@testId", testId);
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var question = new QuestionDisplay
                    {
                        Id = reader.GetInt32("id"),
                        Text = reader.GetString("text")
                    };
                    questions.Add(question);
                }
            }

            foreach (var question in questions)
            {
                question.Answers = GetAnswersByQuestionId(question.Id);
            }
        }
        return questions;
    }

    private List<AnswerDisplay> GetAnswersByQuestionId(int questionId)
    {
        List<AnswerDisplay> answers = new List<AnswerDisplay>();
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var command = new MySqlCommand(
                @"SELECT a.id, a.text, EXISTS(SELECT 1 FROM `Keys` k WHERE k.answer_id = a.id) AS is_correct
              FROM Answers a
              WHERE a.question_id = @questionId", connection);
            command.Parameters.AddWithValue("@questionId", questionId);
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    answers.Add(new AnswerDisplay
                    {
                        Id = reader.GetInt32("id"),
                        Text = reader.GetString("text"),
                        IsCorrect = reader.GetBoolean("is_correct")
                    });
                }
            }
        }
        return answers;
    }

    public List<RangeDisplay> GetRangesByTestId(int testId)
    {
        List<RangeDisplay> ranges = new List<RangeDisplay>();
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var command = new MySqlCommand(
                "SELECT id, min_score, max_score, description FROM TestResultsDescriptions WHERE test_id = @testId",
                connection);
            command.Parameters.AddWithValue("@testId", testId);
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    ranges.Add(new RangeDisplay
                    {
                        Id = reader.GetInt32("id"),
                        MinScore = reader.GetInt32("min_score"),
                        MaxScore = reader.GetInt32("max_score"),
                        Description = reader.GetString("description")
                    });
                }
            }
        }
        return ranges;
    }

    public bool DeleteTest(int id)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var command = new MySqlCommand("DELETE FROM Tests WHERE id = @id", connection);
            command.Parameters.AddWithValue("@id", id);
            try
            {
                return command.ExecuteNonQuery() > 0;
            }
            catch (MySqlException)
            {
                return false; // Обработка ограничений внешнего ключа
            }
        }
    }

    public bool UpdateTest(int id, string name, int themeId)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var command = new MySqlCommand("UPDATE Tests SET name = @name, theme_id = @themeId WHERE id = @id", connection);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@themeId", themeId);
            command.Parameters.AddWithValue("@id", id);
            return command.ExecuteNonQuery() > 0;
        }
    }

    public List<StudentReport> GetTestCompletionReport(string testName)
    {
        var report = new List<StudentReport>();
        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            var command = new MySqlCommand(
                @"SELECT g.name AS group_name, u.name AS full_name, tr.completed_at AS last_date
              FROM TestResults tr
              INNER JOIN Tests t ON tr.test_id = t.id
              INNER JOIN Students u ON tr.student_id = u.id
              INNER JOIN `Groups` g ON u.group_id = g.id
              WHERE t.name = @testName AND tr.completed_at = (SELECT MAX(trs.completed_at) FROM TestResults trs WHERE trs.student_id = u.id AND trs.test_id = t.id)
              ORDER BY g.name, u.name",
                connection);

            command.Parameters.AddWithValue("@testName", testName);

            try
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        report.Add(new StudentReport
                        {
                            GroupName = reader.GetString("group_name"),
                            FullName = reader.GetString("full_name"),
                            LastDate = reader.GetDateTime("last_date")
                        });
                    }
                }
            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine($"MySQL Error: {ex.Message}, TestName: '{testName}'");
                throw;
            }
        }
        return report;
    }

    public int GetUniqueStudentsCount(string testName, DateTime beginDate, DateTime endDate)
    {
        // Настраиваем конечную дату на конец дня
        DateTime endDateWithTime = endDate.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            var command = new MySqlCommand(
                @"SELECT COUNT(DISTINCT tr.student_id)
              FROM TestResults tr
              INNER JOIN Tests t ON tr.test_id = t.id
              WHERE t.name = @testName
              AND tr.completed_at >= @beginDate
              AND tr.completed_at <= @endDate",
                connection);

            command.Parameters.AddWithValue("@testName", testName);
            command.Parameters.AddWithValue("@beginDate", beginDate);
            command.Parameters.AddWithValue("@endDate", endDateWithTime);

            try
            {
                object result = command.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Ошибка MySQL: {ex.Message}, TestName: '{testName}'");
                throw;
            }
        }
    }


    public List<string> GetThemes()
    {
        var themes = new List<string>();
        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            var command = new MySqlCommand("SELECT name FROM Themes ORDER BY name", connection);
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                    themes.Add(reader.GetString("name"));
            }
        }
        return themes;
    }

    public int GetThemeIdByName(string themeName)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            var command = new MySqlCommand("SELECT id FROM Themes WHERE name = @name", connection);
            command.Parameters.AddWithValue("@name", themeName);
            var result = command.ExecuteScalar();
            return result != null ? (int)result : -1;
        }
    }

    public int AddTheme(string themeName)
    {
        if (string.IsNullOrWhiteSpace(themeName)) return -1;
        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            var checkCommand = new MySqlCommand("SELECT COUNT(*) FROM Themes WHERE name = @name", connection);
            checkCommand.Parameters.AddWithValue("@name", themeName);
            if ((long)checkCommand.ExecuteScalar() > 0) return -1;

            var insertCommand = new MySqlCommand("INSERT INTO Themes (name) VALUES (@name); SELECT LAST_INSERT_ID();", connection);
            insertCommand.Parameters.AddWithValue("@name", themeName);
            var result = insertCommand.ExecuteScalar();
            return result != null ? (int)(ulong)result : -1;
        }
    }

    public int AddTest(string testName, int themeId)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            var command = new MySqlCommand("INSERT INTO Tests (name, theme_id) VALUES (@name, @themeId); SELECT LAST_INSERT_ID();", connection);
            command.Parameters.AddWithValue("@name", testName);
            command.Parameters.AddWithValue("@themeId", themeId);
            var result = command.ExecuteScalar();
            return result != null ? (int)(ulong)result : -1;
        }
    }

    public int AddQuestion(string questionText, int testId)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            var command = new MySqlCommand("INSERT INTO Questions (text, test_id) VALUES (@text, @testId); SELECT LAST_INSERT_ID();", connection);
            command.Parameters.AddWithValue("@text", questionText);
            command.Parameters.AddWithValue("@testId", testId);
            var result = command.ExecuteScalar();
            return result != null ? (int)(ulong)result : -1;
        }
    }

    public int AddAnswer(string answerText, int questionId)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            var command = new MySqlCommand("INSERT INTO Answers (text, question_id) VALUES (@text, @questionId); SELECT LAST_INSERT_ID();", connection);
            command.Parameters.AddWithValue("@text", answerText);
            command.Parameters.AddWithValue("@questionId", questionId);
            var result = command.ExecuteScalar();
            return result != null ? (int)(ulong)result : -1;
        }
    }

    public bool AddKey(int answerId)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            // Проверка существования answer_id в Answers
            var checkCommand = new MySqlCommand("SELECT COUNT(*) FROM Answers WHERE id = @answerId", connection);
            checkCommand.Parameters.AddWithValue("@answerId", answerId);
            if ((long)checkCommand.ExecuteScalar() == 0)
            {
                System.Diagnostics.Debug.WriteLine($"Answer ID {answerId} does not exist in Answers table.");
                return false;
            }

            var command = new MySqlCommand("INSERT INTO `Keys` (answer_id) VALUES (@answerId)", connection);
            command.Parameters.AddWithValue("@answerId", answerId);
            try
            {
                return command.ExecuteNonQuery() > 0;
            }
            catch (MySqlException ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in AddKey: {ex.Message}, AnswerId: {answerId}");
                return false;
            }
        }
    }

    public bool AddTestResultDescription(int testId, int minScore, int maxScore, string description)
    {
        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            var command = new MySqlCommand("INSERT INTO TestResultsDescriptions (test_id, min_score, max_score, description) VALUES (@testId, @minScore, @maxScore, @description)", connection);
            command.Parameters.AddWithValue("@testId", testId);
            command.Parameters.AddWithValue("@minScore", minScore);
            command.Parameters.AddWithValue("@maxScore", maxScore);
            command.Parameters.AddWithValue("@description", description);
            return command.ExecuteNonQuery() > 0;
        }
    }

    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int GroupId { get; set; }
    }

    public Student GetStudentByName(string name)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string query = "SELECT id, name, group_id FROM Students WHERE name = @name";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@name", name);
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
        }
        return null;
    }
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

                    };
                }
            }
        }
        return null;
    }

    public List<string> GetTestsByTheme(string themeName)
    {
        var tests = new List<string>();
        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            var command = new MySqlCommand(
                "SELECT t.name FROM Tests t INNER JOIN Themes th ON t.theme_id = th.id WHERE th.name = @themeName ORDER BY t.id",
                connection);
            command.Parameters.AddWithValue("@themeName", themeName);
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    tests.Add(reader.GetString("name"));
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

    public string GetResultDescription(int score, int testId)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var command = new MySqlCommand(
                "SELECT description FROM TestResultsDescriptions WHERE @score BETWEEN min_score AND max_score AND test_id = @testId",
                connection);
            command.Parameters.AddWithValue("@score", score);
            command.Parameters.AddWithValue("@testId", testId);

            object result = command.ExecuteScalar();
            return result != null ? result.ToString() : "Результат не определён для данного теста.";
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

    public List<Student> GetStudents(string findGroup = "")
    {
        var students = new List<Student>();
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string commandFind = "SELECT * FROM Students s WHERE s.group_id = (SELECT id FROM `Groups` g WHERE g.name = @group)";
            string commandNotFind = "SELECT * FROM Students";
            var command = new MySqlCommand(findGroup == "" ? commandNotFind : commandFind, connection);
            if (findGroup != "") command.Parameters.AddWithValue("@group", findGroup);
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