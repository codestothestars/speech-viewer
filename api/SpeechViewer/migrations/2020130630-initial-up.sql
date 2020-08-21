-- Initial database schema and data.

CREATE TABLE [author](
    [id] INT IDENTITY NOT NULL CONSTRAINT [author_id] PRIMARY KEY,
    [first_name] NVARCHAR(MAX) NOT NULL,
    [last_name] NVARCHAR(MAX) NOT NULL
);

CREATE TABLE [speech](
    [id] INT IDENTITY NOT NULL CONSTRAINT [speech_id] PRIMARY KEY,
    [author] INT NOT NULL FOREIGN KEY REFERENCES [author],
    [name] NVARCHAR(MAX) NOT NULL
);

CREATE TABLE [speech_paragraph](
    [id] INT IDENTITY NOT NULL CONSTRAINT [speech_paragraph_id] PRIMARY KEY,
    [index] INT NOT NULL,
    [speech] INT NOT NULL FOREIGN KEY REFERENCES [speech],
    [text] NVARCHAR(MAX) NOT NULL,
    CONSTRAINT [index_speech] UNIQUE([index], [speech])
);

SET IDENTITY_INSERT [author] ON;
INSERT INTO [author]([id], [first_name], [last_name]) VALUES(1, 'Abraham', 'Lincoln');
SET IDENTITY_INSERT [author] OFF;

SET IDENTITY_INSERT [speech] ON;
INSERT INTO [speech]([id], [author], [name]) VALUES(1, 1, 'Gettysburg Address (Bancroft)');
INSERT INTO [speech]([id], [author], [name]) VALUES(2, 1, 'Gettysburg Address (Bliss)');
INSERT INTO [speech]([id], [author], [name]) VALUES(3, 1, 'Gettysburg Address (Everett)');
INSERT INTO [speech]([id], [author], [name]) VALUES(4, 1, 'Gettysburg Address (Hay)');
INSERT INTO [speech]([id], [author], [name]) VALUES(5, 1, 'Gettysburg Address (Nicolay)');
SET IDENTITY_INSERT [speech] OFF;

INSERT INTO [speech_paragraph]([index], [speech], [text]) VALUES(1, 1, 'Four score and seven years ago our fathers brought forth, on this continent, a new nation, conceived in Liberty, and dedicated to the proposition that all men are created equal.');
INSERT INTO [speech_paragraph]([index], [speech], [text]) VALUES(2, 1, 'Now we are engaged in a great civil war, testing whether that nation, or any nation so conceived, and so dedicated, can long endure. We are met on a great battle-field of that war. We have come to dedicate a portion of that field, as a final resting-place for those who here gave their lives, that that nation might live. It is altogether fitting and proper that we should do this.');
INSERT INTO [speech_paragraph]([index], [speech], [text]) VALUES(3, 1, 'But, in a larger sense, we can not dedicate, we can not consecrate we can not hallow this ground. The brave men, living and dead, who struggled here, have consecrated it far above our poor power to add or detract. The world will little note, nor long remember what we say here, but it can never forget what they did here. It is for us the living, rather, to be dedicated here to the unfinished work which they who fought here have thus far so nobly advanced. It is rather for us to be here dedicated to the great task remaining before us that from these honored dead we take increased devotion to that cause for which they here gave the last full measure of devotion - that we here highly resolve that these dead shall not have died in vain that this nation, under God, shall have a new birth of freedom, and that government of the people, by the people, for the people, shall not perish from the earth.');
INSERT INTO [speech_paragraph]([index], [speech], [text]) VALUES(1, 2, 'Four score and seven years ago our fathers brought forth on this continent, a new nation, conceived in Liberty, and dedicated to the proposition that all men are created equal.');
INSERT INTO [speech_paragraph]([index], [speech], [text]) VALUES(2, 2, 'Now we are engaged in a great civil war, testing whether that nation, or any nation so conceived and so dedicated, can long endure. We are met on a great battle-field of that war. We have come to dedicate a portion of that field, as a final resting place for those who here gave their lives that that nation might live. It is altogether fitting and proper that we should do this.');
INSERT INTO [speech_paragraph]([index], [speech], [text]) VALUES(3, 2, 'But, in a larger sense, we can not dedicate -- we can not consecrate -- we can not hallow -- this ground. The brave men, living and dead, who struggled here, have consecrated it, far above our poor power to add or detract. The world will little note, nor long remember what we say here, but it can never forget what they did here. It is for us the living, rather, to be dedicated here to the unfinished work which they who fought here have thus far so nobly advanced. It is rather for us to be here dedicated to the great task remaining before us -- that from these honored dead we take increased devotion to that cause for which they gave the last full measure of devotion -- that we here highly resolve that these dead shall not have died in vain -- that this nation, under God, shall have a new birth of freedom -- and that government of the people, by the people, for the people, shall not perish from the earth.');
INSERT INTO [speech_paragraph]([index], [speech], [text]) VALUES(1, 3, 'Four score and seven years ago our fathers brought forth, upon this continent, a new nation, conceived in Liberty, and dedicated to the proposition that all men are created equal.');
INSERT INTO [speech_paragraph]([index], [speech], [text]) VALUES(2, 3, 'Now we are engaged in a great civil war, testing whether that nation, or any nation so conceived, and so dedicated, can long endure. We are met on a great battle-field of that war. We have come to dedicate a portion of that field, as a final resting-place for those who here gave their lives, that that nation might live. It is altogether fitting and proper that we should do this.');
INSERT INTO [speech_paragraph]([index], [speech], [text]) VALUES(3, 3, 'But, in a larger sense, we can not dedicate, we can not consecrate we can not hallow this ground. The brave men, living and dead, who struggled here, have consecrated it far above our poor power to add or detract. The world will little note, nor long remember what we say here, but it can never forget what they did here.');
INSERT INTO [speech_paragraph]([index], [speech], [text]) VALUES(4, 3, 'It is for us, the living, rather, to be dedicated here to the unfinished work which they who fought here, have, thus far, so nobly advanced. It is rather for us to be here dedicated to the great task remaining before us that from these honored dead we take increased devotion to that cause for which they here gave the last full measure of devotion that we here highly resolve that these dead shall not have died in vain that this nation, under God, shall have a new birth of freedom and that government of the people, by the people, for the people, shall not perish from the earth.');
INSERT INTO [speech_paragraph]([index], [speech], [text]) VALUES(1, 4, 'Four score and seven years ago our fathers brought forth, upon this continent, a new nation, conceived in Liberty, and dedicated to the proposition that all men are created equal.');
INSERT INTO [speech_paragraph]([index], [speech], [text]) VALUES(2, 4, 'Now we are engaged in a great civil war, testing whether that nation, or any nation so conceived, and so dedicated, can long endure. We are met here on a great battlefield of that war. We have come to dedicate a portion of it, as a final resting place for those who here gave their lives that that nation might live. It is altogether fitting and proper that we should do this.');
INSERT INTO [speech_paragraph]([index], [speech], [text]) VALUES(3, 4, 'But in a larger sense, we can not dedicate we can not consecrate we can not hallow this ground. The brave men, living and dead, who struggled here, have consecrated it far above our poor power to add or detract. The world will little note, nor long remember, what we say here, but can never forget what they did here.');
INSERT INTO [speech_paragraph]([index], [speech], [text]) VALUES(4, 4, 'It is for us, the living, rather to be dedicated here to the unfinished work which they have, thus far, so nobly carried on. It is rather for us to be here dedicated to the great task remaining before us that from these honored dead we take increased devotion to that cause for which they gave the last full measure of devotion that we here highly resolve that these dead shall not have died in vain; that this nation shall have a new birth of freedom; and that this government of the people, by the people, for the people, shall not perish from the earth.');
INSERT INTO [speech_paragraph]([index], [speech], [text]) VALUES(1, 5, 'Four score and seven years ago our fathers brought forth, upon this continent, a new nation, conceived in liberty, and dedicated to the proposition that all men are created equal.');
INSERT INTO [speech_paragraph]([index], [speech], [text]) VALUES(2, 5, 'Now we are engaged in a great civil war, testing whether that nation, or any nation so conceived, and so dedicated, can long endure. We are met on a great battle field of that war. We come to dedicate a portion of it, as a final resting place for those who died here, that the nation might live. This we may, in all propriety do.');
INSERT INTO [speech_paragraph]([index], [speech], [text]) VALUES(3, 5, 'But, in a larger sense, we can not dedicate we can not consecrate we can not hallow, this ground The brave men, living and dead, who struggled here, have hallowed it, far above our poor power to add or detract. The world will little note, nor long remember what we say here; while it can never forget what they did here.');
INSERT INTO [speech_paragraph]([index], [speech], [text]) VALUES(4, 5, 'It is rather for us, the living, we here be dedicated to the great task remaining before us that, from these honored dead we take increased devotion to that cause for which they here, gave the last full measure of devotion that we here highly resolve these dead shall not have died in vain; that the nation, shall have a new birth of freedom, and that government of the people, by the people, for the people, shall not perish from the earth.');
