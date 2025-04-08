<?php

function generatePublicToken($length = 32) {
    return bin2hex(random_bytes($length));
}

$publicToken = generatePublicToken();
$expiresAt = new DateTime('+1 hour'); // Token élettartama

$stmt = $conn->prepare("INSERT INTO tokens (user_id, access_token, expires_at) VALUES (NULL, ?, ?)");
$stmt->execute([$publicToken, $expiresAt->format('Y-m-d H:i:s')]);

$data = array(
    'access_token' => $publicToken,
    'expiresAt' => $expiresAt,
    'level' => 'public'
);

Response::success($data);
?>