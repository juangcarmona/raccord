import { useCallback, useEffect, useRef, useState } from 'react';

export type QueryState<T> =
  | { status: 'loading' }
  | { status: 'error'; error: unknown }
  | { status: 'success'; data: T };

type Settled<T> = { request: string } & ({ status: 'error'; error: unknown } | { status: 'success'; data: T });

/**
 * Loads data for a view. `key` identifies what is loaded; changing it, or calling `reload`, loads again.
 * Responses that belong to an earlier request are ignored.
 */
export function useQuery<T>(key: string, load: () => Promise<T>): QueryState<T> & { reload: () => void } {
  const [version, setVersion] = useState(0);
  const [settled, setSettled] = useState<Settled<T> | null>(null);
  const loader = useRef(load);
  const request = `${key}#${version}`;

  useEffect(() => {
    loader.current = load;
  });

  useEffect(() => {
    let current = true;
    loader.current().then(
      (data) => {
        if (current) setSettled({ request, status: 'success', data });
      },
      (error: unknown) => {
        if (current) setSettled({ request, status: 'error', error });
      },
    );
    return () => {
      current = false;
    };
  }, [request]);

  const reload = useCallback(() => {
    setVersion((value) => value + 1);
  }, []);

  if (settled?.request !== request) {
    return { status: 'loading', reload };
  }
  return settled.status === 'success'
    ? { status: 'success', data: settled.data, reload }
    : { status: 'error', error: settled.error, reload };
}
