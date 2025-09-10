import { render, screen } from '@testing-library/react'
import { HeroUIProvider } from '@heroui/react'
import Home from '../app/page'

// Mock HeroUI components to avoid issues with framer-motion and complex animations
jest.mock('@heroui/react', () => ({
  ...jest.requireActual('@heroui/react'),
  Button: ({ children, color, size, variant, ...props }: any) => (
    <button data-color={color} data-size={size} data-variant={variant} {...props}>
      {children}
    </button>
  ),
  Card: ({ children, ...props }: any) => <div data-testid="card" {...props}>{children}</div>,
  CardBody: ({ children, ...props }: any) => <div data-testid="card-body" {...props}>{children}</div>,
  CardHeader: ({ children, ...props }: any) => <div data-testid="card-header" {...props}>{children}</div>,
  Navbar: ({ children, ...props }: any) => <nav data-testid="navbar" {...props}>{children}</nav>,
  NavbarBrand: ({ children, ...props }: any) => <div data-testid="navbar-brand" {...props}>{children}</div>,
  NavbarContent: ({ children, justify, ...props }: any) => (
    <div data-testid="navbar-content" data-justify={justify} {...props}>{children}</div>
  ),
  Avatar: ({ isBordered, size, src, ...props }: any) => (
    <img data-bordered={isBordered} data-size={size} src={src} alt="avatar" {...props} />
  ),
  HeroUIProvider: ({ children }: any) => <div>{children}</div>,
}))

const renderWithProvider = (ui: React.ReactElement) => {
  return render(<HeroUIProvider>{ui}</HeroUIProvider>)
}

describe('Home Page', () => {
  it('renders the main heading', () => {
    renderWithProvider(<Home />)
    expect(screen.getByText('Welcome to Events Coordinator')).toBeInTheDocument()
  })

  it('renders the subtitle', () => {
    renderWithProvider(<Home />)
    expect(screen.getByText(/A modern Next.js 15\+ application powered by HeroUI components/)).toBeInTheDocument()
  })

  it('renders navigation with Events Coordinator brand', () => {
    renderWithProvider(<Home />)
    expect(screen.getByText('Events Coordinator')).toBeInTheDocument()
  })

  it('renders call-to-action buttons', () => {
    renderWithProvider(<Home />)
    expect(screen.getByText('Get Started')).toBeInTheDocument()
    expect(screen.getByText('Learn More')).toBeInTheDocument()
  })

  it('renders feature cards', () => {
    renderWithProvider(<Home />)
    expect(screen.getByText('Next.js 15+')).toBeInTheDocument()
    expect(screen.getByText('Modern Framework')).toBeInTheDocument()
    expect(screen.getByText('HeroUI')).toBeInTheDocument()
    expect(screen.getByText('Beautiful Components')).toBeInTheDocument()
    expect(screen.getByText('TypeScript')).toBeInTheDocument()
    expect(screen.getByText('Type Safety')).toBeInTheDocument()
  })

  it('renders HeroUI components demo section', () => {
    renderWithProvider(<Home />)
    expect(screen.getByText('HeroUI Components Demo')).toBeInTheDocument()
    expect(screen.getByText('Primary')).toBeInTheDocument()
    expect(screen.getByText('Secondary')).toBeInTheDocument()
    expect(screen.getByText('Success')).toBeInTheDocument()
    expect(screen.getByText('Warning')).toBeInTheDocument()
    expect(screen.getByText('Danger')).toBeInTheDocument()
  })

  it('renders avatar in navigation', () => {
    renderWithProvider(<Home />)
    expect(screen.getByAltText('avatar')).toBeInTheDocument()
  })
})