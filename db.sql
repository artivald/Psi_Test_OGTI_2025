-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Хост: 127.0.0.1:3306
-- Время создания: Июн 08 2025 г., 08:46
-- Версия сервера: 8.0.30
-- Версия PHP: 7.2.34

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- База данных: `PsychologicalTests`
--

-- --------------------------------------------------------

--
-- Структура таблицы `Accounts`
--

CREATE TABLE `Accounts` (
  `id` int NOT NULL,
  `username` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `password` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Дамп данных таблицы `Accounts`
--

INSERT INTO `Accounts` (`id`, `username`, `password`) VALUES
(1, 'KuznetchenkoMA', '1234567890');

-- --------------------------------------------------------

--
-- Структура таблицы `Answers`
--

CREATE TABLE `Answers` (
  `id` int NOT NULL,
  `text` text CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `question_id` int NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Дамп данных таблицы `Answers`
--

INSERT INTO `Answers` (`id`, `text`, `question_id`) VALUES
(19, 'Иванов', 9),
(20, 'Василий', 9),
(21, 'Федор', 9),
(22, 'Семен', 9),
(23, 'Петр', 9),
(24, 'Маленький', 10),
(25, 'Дряхлый', 10),
(26, 'Старый', 10),
(27, 'Изношенный', 10),
(28, 'Ветхий', 10),
(29, 'Постепенно', 11),
(30, 'Скоро', 11),
(31, 'Быстро', 11),
(32, 'Поспешно', 11),
(33, 'Торопливо', 11),
(34, 'Почва', 12),
(35, 'Лист', 12),
(36, 'Кора', 12),
(37, 'Ветви', 12),
(38, 'Сук', 12),
(39, 'Понимать', 13),
(40, 'Ненавидеть', 13),
(41, 'Презирать', 13),
(42, 'Негодовать', 13),
(43, 'Возмущаться', 13),
(44, 'Голубой', 14),
(45, 'Темный', 14),
(46, 'Светлый', 14),
(47, 'Яркий', 14),
(48, 'Тусклый', 14),
(49, 'Сторожка', 15),
(50, 'Гнездо', 15),
(51, 'Нора', 15),
(52, 'Курятник', 15),
(53, 'Берлога', 15),
(54, 'Волнение', 16),
(55, 'Неудача', 16),
(56, 'Поражение', 16),
(57, 'Провал', 16),
(58, 'Крах', 16),
(59, 'Спокойствие', 17),
(60, 'Успех', 17),
(61, 'Неудача', 17),
(62, 'Удача', 17),
(63, 'Выигрыш', 17),
(64, 'Землетрясение', 18),
(65, 'Грабеж', 18),
(66, 'Кража', 18),
(67, 'Поджог', 18),
(68, 'Нападение', 18),
(69, 'Сало', 19),
(70, 'Молоко', 19),
(71, 'Сыр', 19),
(72, 'Сметана', 19),
(73, 'Простокваша', 19),
(74, 'Горький', 20),
(75, 'Глубокий', 20),
(76, 'Низкий', 20),
(77, 'Светлый', 20),
(78, 'Высокий', 20),
(79, 'Дым', 21),
(80, 'Хата', 21),
(81, 'Печь', 21),
(82, 'Хлев', 21),
(83, 'Будка', 21),
(84, 'Сирень', 22),
(85, 'Береза', 22),
(86, 'Сосна', 22),
(87, 'Дуб', 22),
(88, 'Ель', 22),
(89, 'Вечер', 23),
(90, 'Секунда', 23),
(91, 'Час', 23),
(92, 'Год', 23),
(93, 'Неделя', 23),
(119, 'Да', 29),
(120, 'Нет', 29),
(121, 'Да', 30),
(122, 'Нет', 30),
(123, 'Да', 31),
(124, 'Нет', 31),
(125, 'Да', 32),
(126, 'Нет', 32),
(127, 'Да', 33),
(128, 'Нет', 33),
(129, 'Да', 34),
(130, 'Нет', 34),
(131, 'Да', 35),
(132, 'Нет', 35),
(133, 'Да', 36),
(134, 'Нет', 36),
(135, 'Да', 37),
(136, 'Нет', 37),
(137, 'Да', 38),
(138, 'Нет', 38),
(139, 'Да', 39),
(140, 'Нет', 39),
(141, 'Да', 40),
(142, 'Нет', 40),
(143, 'Да', 41),
(144, 'Нет', 41),
(145, 'Да', 42),
(146, 'Нет', 42),
(147, 'Да', 43),
(148, 'Нет', 43),
(149, 'Да', 44),
(150, 'Нет', 44),
(151, 'Да', 45),
(152, 'Нет', 45),
(153, 'Да', 46),
(154, 'Нет', 46),
(155, 'Да', 47),
(156, 'Нет', 47),
(157, 'Да', 48),
(158, 'Нет', 48),
(159, 'Да', 49),
(160, 'Нет', 49),
(161, 'Да', 50),
(162, 'Нет', 50),
(163, 'Да', 51),
(164, 'Нет', 51),
(165, 'Да', 52),
(166, 'Нет', 52),
(167, 'Да', 53),
(168, 'Нет', 53),
(169, '1', 54),
(170, '2', 54);

-- --------------------------------------------------------

--
-- Структура таблицы `Groups`
--

CREATE TABLE `Groups` (
  `id` int NOT NULL,
  `name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Дамп данных таблицы `Groups`
--

INSERT INTO `Groups` (`id`, `name`) VALUES
(1, '21ИСП-1');

-- --------------------------------------------------------

--
-- Структура таблицы `Keys`
--

CREATE TABLE `Keys` (
  `id` int NOT NULL,
  `answer_id` int NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Дамп данных таблицы `Keys`
--

INSERT INTO `Keys` (`id`, `answer_id`) VALUES
(1, 19),
(2, 24),
(3, 29),
(4, 34),
(5, 39),
(6, 44),
(7, 49),
(8, 54),
(9, 59),
(10, 64),
(11, 69),
(12, 74),
(13, 79),
(14, 84),
(15, 89),
(16, 119),
(17, 121),
(18, 123),
(19, 125),
(20, 127),
(21, 129),
(22, 131),
(23, 133),
(24, 135),
(25, 137),
(26, 139),
(27, 141),
(28, 143),
(29, 145),
(30, 147),
(31, 149),
(32, 151),
(33, 153),
(34, 155),
(35, 157),
(36, 159),
(37, 161),
(38, 163),
(39, 165),
(40, 167),
(41, 169);

-- --------------------------------------------------------

--
-- Структура таблицы `Questions`
--

CREATE TABLE `Questions` (
  `id` int NOT NULL,
  `text` text CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `test_id` int NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Дамп данных таблицы `Questions`
--

INSERT INTO `Questions` (`id`, `text`, `test_id`) VALUES
(9, 'Василий, Федор, Семен, Иванов, Петр', 3),
(10, 'Дряхлый, маленький, старый, изношенный, ветхий', 3),
(11, 'Скоро, быстро, поспешно, постепенно, торопливо', 3),
(12, 'Лист, почва, кора, ветви, сук', 3),
(13, 'Ненавидеть, презирать, негодовать, возмущаться, понимать', 3),
(14, 'Темный, светлый, голубой, яркий, тусклый', 3),
(15, 'Гнездо, нора, курятник, сторожка, берлога', 3),
(16, 'Неудача, волнение, поражение, провал, крах', 3),
(17, 'Успех, неудача, удача, выигрыш, спокойствие', 3),
(18, 'Грабеж, кража, землетрясение, поджог, нападение', 3),
(19, 'Молоко, сыр, сметана, сало, простокваша', 3),
(20, 'Глубокий, низкий, светлый, высокий, горький', 3),
(21, 'Хата, печь, дым, хлев, будка', 3),
(22, 'Береза, сосна, дуб, сирень, ель', 3),
(23, 'Секунда, час, год, вечер, неделя', 3),
(29, '1. Собеседник не дает мне шанса высказаться, у меня есть, что ска-зать, но нет возможности вставить слово.', 4),
(30, '2. Собеседник постоянно прерывает меня во время беседы.', 4),
(31, '3. Собеседник никогда не смотрит в лицо во время разговора, и я не уверен, слушает ли он меня.', 4),
(32, '4. Разговор с таким партнером часто вызывает чувство пусто и траты времени.', 4),
(33, '5. Собеседник постоянно суетится, карандаш и бумаге занимают его больше, чем мои слова.', 4),
(34, '6. Собеседник никогда не улыбается. У меня возникает чувство недовольства и тревоги.', 4),
(35, '7. Собеседник отвлекает меня вопросами и комментариями.', 4),
(36, '8. Что бы я ни сказал, собеседник всегда охлаждает мой пыл.', 4),
(37, '9. Собеседник всегда старается опровергнуть меня.', 4),
(38, '10. Собеседник передергивает смысл моих слов и вкладывает в них другое содержание.', 4),
(39, '11. Когда я задаю вопрос, собеседник заставляет меня защищать-ся.', 4),
(40, '12. Иногда собеседник переспрашивает меня, делая вид. Что, не расслышал.', 4),
(41, '13. Собеседник, не дослушав до конца, перебивает меня лишь за-тем, чтобы согласиться.Вопрос 13', 4),
(42, '14. Собеседник при разговоре сосредоточенно занимается посто-ронним: играет сигаретой, протирает стекла и т.д., и я твердо уверен, что он при этом невнимателен.', 4),
(43, '15. Собеседник делает выводы за меня.', 4),
(44, '16. Собеседник всегда пытается вставить слово в мое повествова-ние.', 4),
(45, '17. Собеседник всегда смотрит на меня очень внимательно. не ми-гая.', 4),
(46, '18. Собеседник смотрит на меня, как бы оценивая. Это меня бес-покоит.', 4),
(47, '19. Когда я предлагаю что-нибудь новое, собеседник говорит, что он думает так же.', 4),
(48, '20. Собеседник переигрывает, показывая, что интересуется бесе-дой, слишком часто кивает головой, ахает и поддакивает.', 4),
(49, '21. Когда я говорю о серьезном, а собеседник вставляет смешные истории, шуточки, анекдоты.', 4),
(50, '22. Собеседник часто глядит на часы во время разговора.', 4),
(51, '23. Когда я вхожу в кабинет, он бросает все дела и все внимание обращает на меня.', 4),
(52, '24. Собеседник ведет себя так, будто я мешаю ему делать что-нибудь важное.', 4),
(53, '25. Собеседник требует, чтобы все соглашались с ним. Любое его высказывание завершается вопросом: «\'Вы тоже так думаете?» или «Вы с этим не согласны?»', 4);

-- --------------------------------------------------------

--
-- Структура таблицы `Students`
--

CREATE TABLE `Students` (
  `id` int NOT NULL,
  `name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `group_id` int NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Дамп данных таблицы `Students`
--

INSERT INTO `Students` (`id`, `name`, `group_id`) VALUES
(1, 'Дубинина Екатерина Владимировна', 1);

-- --------------------------------------------------------

--
-- Структура таблицы `TestBlocks`
--

CREATE TABLE `TestBlocks` (
  `test_id` int NOT NULL,
  `block_id` int NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Дамп данных таблицы `TestBlocks`
--

INSERT INTO `TestBlocks` (`test_id`, `block_id`) VALUES
(3, 3);

-- --------------------------------------------------------

--
-- Структура таблицы `TestResults`
--

CREATE TABLE `TestResults` (
  `id` int NOT NULL,
  `student_id` int NOT NULL,
  `test_id` int DEFAULT '0',
  `score` int NOT NULL,
  `completed_at` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Дамп данных таблицы `TestResults`
--

INSERT INTO `TestResults` (`id`, `student_id`, `test_id`, `score`, `completed_at`) VALUES
(1, 1, 3, 4, '2025-05-16 21:28:44'),
(2, 1, 4, 4, '2025-06-08 08:17:49'),
(3, 1, 4, 25, '2025-06-08 08:23:11'),
(4, 1, 4, 25, '2025-06-08 08:24:10'),
(5, 1, 3, 0, '2025-06-08 08:29:31'),
(6, 1, 3, 0, '2025-06-08 08:34:34'),
(7, 1, 4, 2, '2025-06-08 08:37:08'),
(8, 1, 4, 0, '2025-06-08 08:42:50');

-- --------------------------------------------------------

--
-- Структура таблицы `TestResultsDescriptions`
--

CREATE TABLE `TestResultsDescriptions` (
  `id` int NOT NULL,
  `test_id` int NOT NULL,
  `min_score` int NOT NULL,
  `max_score` int NOT NULL,
  `description` text CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Дамп данных таблицы `TestResultsDescriptions`
--

INSERT INTO `TestResultsDescriptions` (`id`, `test_id`, `min_score`, `max_score`, `description`) VALUES
(4, 3, 0, 5, 'Низкий уровень способности к обобщению и абстрагированию. Трудности в выделении существенных признаков.'),
(5, 3, 6, 10, 'Средний уровень развития обобщения. Способность к абстрагированию проявляется ситуативно.'),
(6, 3, 11, 14, 'Хороший уровень способности к обобщению. Умение выделять существенные признаки развито.'),
(7, 3, 15, 15, 'Отличные способности к обобщению и абстрагированию.'),
(8, 4, 18, 25, 'Вы плохой собеседник. Вам необходимо работать над собой и учиться слушать.'),
(9, 4, 10, 17, 'Вам присущи некоторые недостатки. Вы критически относитесь к высказываниям. Вам еще недостает некоторых достоинств хорошего собеседника, избегайте поспешных выводов, не заостряйте внимание на манере говорить, не притворяйтесь, не ищите скрытый смысл сказанного, не монополизируйте разговор.'),
(10, 4, 3, 9, 'Вы хороший собеседник, но иногда отказываете партнеру в полном внимании. Повторяйте вежливо его высказывания, дайте ему время раскрыть свою мысль полностью, приспосабливайте свой темп мышления к его речи и можете быть уверены, что общаться с Вами будет еще приятнее.'),
(11, 4, 0, 2, 'Вы отличный собеседник. Вы умеете слушать. Ваш стиль общения может стать примером для окружающих.');

-- --------------------------------------------------------

--
-- Структура таблицы `Tests`
--

CREATE TABLE `Tests` (
  `id` int NOT NULL,
  `name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `theme_id` int NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Дамп данных таблицы `Tests`
--

INSERT INTO `Tests` (`id`, `name`, `theme_id`) VALUES
(3, 'Тест «Исключение лишнего понятия»', 2),
(4, 'Методика «Оценка коммуникативных умений»', 3);

-- --------------------------------------------------------

--
-- Структура таблицы `Themes`
--

CREATE TABLE `Themes` (
  `id` int NOT NULL,
  `name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Дамп данных таблицы `Themes`
--

INSERT INTO `Themes` (`id`, `name`) VALUES
(2, 'Познавательные процессы'),
(3, 'Личность в системе');

--
-- Индексы сохранённых таблиц
--

--
-- Индексы таблицы `Accounts`
--
ALTER TABLE `Accounts`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `username` (`username`);

--
-- Индексы таблицы `Answers`
--
ALTER TABLE `Answers`
  ADD PRIMARY KEY (`id`),
  ADD KEY `question_id` (`question_id`);

--
-- Индексы таблицы `Groups`
--
ALTER TABLE `Groups`
  ADD PRIMARY KEY (`id`);

--
-- Индексы таблицы `Keys`
--
ALTER TABLE `Keys`
  ADD PRIMARY KEY (`id`),
  ADD KEY `answer_id` (`answer_id`);

--
-- Индексы таблицы `Questions`
--
ALTER TABLE `Questions`
  ADD PRIMARY KEY (`id`),
  ADD KEY `test_id` (`test_id`);

--
-- Индексы таблицы `Students`
--
ALTER TABLE `Students`
  ADD PRIMARY KEY (`id`),
  ADD KEY `group_id` (`group_id`);

--
-- Индексы таблицы `TestBlocks`
--
ALTER TABLE `TestBlocks`
  ADD PRIMARY KEY (`test_id`,`block_id`),
  ADD KEY `block_id` (`block_id`);

--
-- Индексы таблицы `TestResults`
--
ALTER TABLE `TestResults`
  ADD PRIMARY KEY (`id`),
  ADD KEY `student_id` (`student_id`),
  ADD KEY `test_id` (`test_id`);

--
-- Индексы таблицы `TestResultsDescriptions`
--
ALTER TABLE `TestResultsDescriptions`
  ADD PRIMARY KEY (`id`),
  ADD KEY `test_id` (`test_id`);

--
-- Индексы таблицы `Tests`
--
ALTER TABLE `Tests`
  ADD PRIMARY KEY (`id`),
  ADD KEY `fk_theme` (`theme_id`);

--
-- Индексы таблицы `Themes`
--
ALTER TABLE `Themes`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT для сохранённых таблиц
--

--
-- AUTO_INCREMENT для таблицы `Accounts`
--
ALTER TABLE `Accounts`
  MODIFY `id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT для таблицы `Answers`
--
ALTER TABLE `Answers`
  MODIFY `id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=171;

--
-- AUTO_INCREMENT для таблицы `Groups`
--
ALTER TABLE `Groups`
  MODIFY `id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT для таблицы `Keys`
--
ALTER TABLE `Keys`
  MODIFY `id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=42;

--
-- AUTO_INCREMENT для таблицы `Questions`
--
ALTER TABLE `Questions`
  MODIFY `id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=55;

--
-- AUTO_INCREMENT для таблицы `Students`
--
ALTER TABLE `Students`
  MODIFY `id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT для таблицы `TestResults`
--
ALTER TABLE `TestResults`
  MODIFY `id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT для таблицы `TestResultsDescriptions`
--
ALTER TABLE `TestResultsDescriptions`
  MODIFY `id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=13;

--
-- AUTO_INCREMENT для таблицы `Tests`
--
ALTER TABLE `Tests`
  MODIFY `id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT для таблицы `Themes`
--
ALTER TABLE `Themes`
  MODIFY `id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- Ограничения внешнего ключа сохраненных таблиц
--

--
-- Ограничения внешнего ключа таблицы `Answers`
--
ALTER TABLE `Answers`
  ADD CONSTRAINT `answers_ibfk_1` FOREIGN KEY (`question_id`) REFERENCES `Questions` (`id`);

--
-- Ограничения внешнего ключа таблицы `Keys`
--
ALTER TABLE `Keys`
  ADD CONSTRAINT `keys_ibfk_1` FOREIGN KEY (`answer_id`) REFERENCES `Answers` (`id`);

--
-- Ограничения внешнего ключа таблицы `Questions`
--
ALTER TABLE `Questions`
  ADD CONSTRAINT `questions_ibfk_1` FOREIGN KEY (`test_id`) REFERENCES `Tests` (`id`);

--
-- Ограничения внешнего ключа таблицы `Students`
--
ALTER TABLE `Students`
  ADD CONSTRAINT `students_ibfk_1` FOREIGN KEY (`group_id`) REFERENCES `Groups` (`id`);

--
-- Ограничения внешнего ключа таблицы `TestBlocks`
--
ALTER TABLE `TestBlocks`
  ADD CONSTRAINT `testblocks_ibfk_1` FOREIGN KEY (`test_id`) REFERENCES `Tests` (`id`),
  ADD CONSTRAINT `testblocks_ibfk_2` FOREIGN KEY (`block_id`) REFERENCES `Blocks` (`id`);

--
-- Ограничения внешнего ключа таблицы `TestResults`
--
ALTER TABLE `TestResults`
  ADD CONSTRAINT `testresults_ibfk_1` FOREIGN KEY (`student_id`) REFERENCES `Students` (`id`),
  ADD CONSTRAINT `testresults_ibfk_2` FOREIGN KEY (`test_id`) REFERENCES `Tests` (`id`);

--
-- Ограничения внешнего ключа таблицы `TestResultsDescriptions`
--
ALTER TABLE `TestResultsDescriptions`
  ADD CONSTRAINT `testresultsdescriptions_ibfk_1` FOREIGN KEY (`test_id`) REFERENCES `Tests` (`id`);

--
-- Ограничения внешнего ключа таблицы `Tests`
--
ALTER TABLE `Tests`
  ADD CONSTRAINT `fk_theme` FOREIGN KEY (`theme_id`) REFERENCES `Themes` (`id`) ON DELETE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
