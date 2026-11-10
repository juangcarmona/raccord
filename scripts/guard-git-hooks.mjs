#!/usr/bin/env node

let input = '';
try {
  for await (const chunk of process.stdin) input += chunk;
  const command = JSON.parse(input)?.tool_input?.command;
  if (typeof command !== 'string') process.exit(0);

  const huskyDisabled = /(?:^|[\s;&|])(?:export\s+)?HUSKY\s*=\s*(?:0|['"]0['"])(?=$|[\s;&|])/u.test(command);
  const gitCalls = command.matchAll(/\bgit(?:\s+(?:-C\s+\S+|-c\s+\S+|--git-dir(?:=\S+|\s+\S+)|--work-tree(?:=\S+|\s+\S+)))*\s+(commit|push)\b([^;&|\n]*)/gu);
  const bypassesGitHook = [...gitCalls].some(([, action, args]) =>
    /(?:^|\s)--no-verify(?=$|\s)/u.test(args) ||
    (action === 'commit' && /(?:^|\s)-n(?=$|\s)/u.test(args)),
  );

  if (huskyDisabled || bypassesGitHook) {
    process.stderr.write('Git hooks must run. Remove --no-verify, commit -n, or HUSKY=0 and retry the command.\n');
    process.exit(2);
  }
} catch {
  // A guard failure must not block the coding session.
}
